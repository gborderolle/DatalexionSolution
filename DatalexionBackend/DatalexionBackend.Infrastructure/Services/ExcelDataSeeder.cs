using ClosedXML.Excel;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace DatalexionBackend.Infrastructure.Services
{
    public static class ExcelDataSeeder
    {
        public static async Task LoadDataFromExcel(IServiceProvider serviceProvider, string wwwrootPath)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    using (var context = services.GetRequiredService<ContextDB>())
                    {
                        var filePath = Path.Combine(wwwrootPath, "Circuitos_upload.xlsx");
                        var workbook = new XLWorkbook(filePath);
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed();

                        var headers = worksheet.Row(1).CellsUsed().Select(c => c.Value.ToString()).ToList();
                        if (headers.Count == 0)
                        {
                            return;
                        }
                        else
                        {
                            var tablesToClear = new List<string> { "CircuitParty", "CircuitSlate", "CircuitDelegado", "Circuit", "Municipality" };
                            foreach (var tableName in tablesToClear)
                            {
                                await context.Database.ExecuteSqlRawAsync($"DELETE FROM {tableName};");
                                // En SQL Server, también podrías considerar reiniciar los contadores de IDENTITY si es necesario:
                                // await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{tableName}', RESEED, 0);");
                            }
                        }

                        int numberIndex = headers.IndexOf("CIRCUITO") + 1;
                        int nameIndex = headers.IndexOf("NOMBRE") + 1;
                        int municipalityIndex = headers.IndexOf("MUNICIPIO") + 1;
                        int addressIndex = headers.IndexOf("DIRECCIÓN") + 1;
                        int provinceIndex = headers.IndexOf("PROVINCIA") + 1; // Asegúrate de que esta es la columna correcta

                        // ToDo: Hardcodeado
                        var provinceId = 1; // Asumiendo que el provinceId es 1 para este ejemplo
                        var delegadoId = 1; // Asumiendo que el delegadoId es 1 para este ejemplo

                        var delegado = await context.Delegado.FirstOrDefaultAsync(p => p.Id == delegadoId);

                        // Agregando municipios
                        var municipalitiesToAdd = new List<Municipality>();
                        foreach (var row in rows.Skip(1))
                        {
                            var municipalityName = row.Cell(municipalityIndex).Value.ToString().Trim();

                            var province = await context.Province.FirstOrDefaultAsync(p => p.Id == provinceId);
                            if (province != null && !municipalitiesToAdd.Any(m => m.Name == municipalityName))
                            {
                                municipalitiesToAdd.Add(new Municipality
                                {
                                    Name = municipalityName,
                                    ProvinceId = provinceId,
                                    Province = province,
                                    DelegadoId = delegadoId,
                                    Delegado = delegado
                                });
                            }
                        }

                        // Guardar todos los municipios agregados
                        context.Municipality.AddRange(municipalitiesToAdd);
                        await context.SaveChangesAsync();

                        if (delegado != null)
                        {
                            delegado.ListMunicipalities = municipalitiesToAdd;
                            context.Delegado.Update(delegado);
                        }

                        // Creando el Dictionary de MunicipalityIds
                        var municipalityIds = municipalitiesToAdd.ToDictionary(m => m.Name, m => m.Id);

                        // Agregando circuitos y asociando ListCircuitSlates y a ListCircuitParties
                        foreach (var row in rows.Skip(1))
                        {
                            int circuitNumber = int.TryParse(row.Cell(numberIndex).Value.ToString().Trim(), out int result) ? result : 0;
                            var circuitName = row.Cell(nameIndex).Value.ToString().Trim();
                            var municipalityName = row.Cell(municipalityIndex).Value.ToString().Trim();
                            var address = row.Cell(addressIndex).Value.ToString().Trim();
                            string latLong = await GetLatLongFromAddress(address);

                            if (municipalityIds.ContainsKey(municipalityName))
                            {
                                var municipality1 = await context.Municipality.FirstOrDefaultAsync(p => p.Id == municipalityIds[municipalityName]);
                                if (municipality1 != null)
                                {

                                    var circuit = new Circuit
                                    {
                                        Name = circuitName,
                                        Number = circuitNumber,
                                        Address = address,
                                        LatLong = latLong,
                                        MunicipalityId = municipality1.Id,
                                        Municipality = municipality1,
                                        LastUpdateDelegadoId = delegadoId
                                    };

                                    context.Circuit.Add(circuit);
                                    await context.SaveChangesAsync();

                                    if (delegado != null)
                                    {
                                        var circuitDelegado = new CircuitDelegado
                                        {
                                            CircuitId = circuit.Id,
                                            DelegadoId = delegado.Id
                                        };

                                        context.CircuitDelegado.Add(circuitDelegado);
                                        await context.SaveChangesAsync();
                                    }

                                    // Asociar el circuito con todos los slates de provinceId 1
                                    var slatesInProvince = await context.Slate
                                        .Where(s => s.ProvinceId == provinceId)
                                        .ToListAsync();

                                    foreach (var slate in slatesInProvince)
                                    {
                                        var circuitSlate = new CircuitSlate
                                        {
                                            CircuitId = circuit.Id,
                                            SlateId = slate.Id
                                        };
                                        context.CircuitSlate.Add(circuitSlate);
                                    }
                                    await context.SaveChangesAsync();

                                    // Asociar el circuito con todos los parties
                                    var parties = await context.Party.ToListAsync();
                                    foreach (var slate in parties)
                                    {
                                        var circuitParty = new CircuitParty
                                        {
                                            CircuitId = circuit.Id,
                                            PartyId = slate.Id
                                        };
                                        context.CircuitParty.Add(circuitParty);
                                    }
                                }
                            }
                        }
                        await context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SeedVotes(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                Random random = new();
                using (var context = services.GetRequiredService<ContextDB>())
                {
                    var client = context.Client.FirstOrDefault();
                    if (client == null)
                    {
                        return;
                    }

                    var firstThreeCircuits = context.Circuit
                        .Include(c => c.ListCircuitSlates)
                        .Include(c => c.ListCircuitParties)
                        .OrderBy(c => c.Number)
                        .Take(3)
                        .ToList();

                    var wingsOfClientParty = context.Wing
                        .Where(wing => wing.PartyId == client.PartyId)
                        .Select(wing => wing.Id)
                        .ToList();

                    if (firstThreeCircuits.Any())
                    {
                        foreach (var circuit in firstThreeCircuits)
                        {
                            int slateTotalVotes = 0;

                            // Añade votos a las slates del circuito
                            foreach (var slate in circuit.ListCircuitSlates)
                            {
                                int randomVotes = random.Next(1, 30); // Genera un número aleatorio entre 1 y 100
                                int vote = (slate.Votes ?? 0) + randomVotes;
                                slate.Votes = vote;
                                // slateTotalVotes += vote;

                                var slate1 = context.Slate.FirstOrDefault(mySlate => mySlate.Id == slate.SlateId);
                                if (slate1 != null)
                                {
                                    if (wingsOfClientParty.Contains(slate1.WingId))
                                    {
                                        slateTotalVotes += vote; // Suma solo si pertenece al partido del cliente
                                    }
                                }
                            }

                            // Añade votos a los partidos del circuito
                            foreach (var party in circuit.ListCircuitParties)
                            {
                                if (party.PartyId == client.PartyId)
                                {
                                    party.Votes = slateTotalVotes;
                                    continue;
                                }
                                int randomVotes = random.Next(1, 150); // Genera un número aleatorio entre 1 y 100
                                int vote = (party.Votes ?? 0) + randomVotes;
                                party.Votes = vote;
                            }

                            // Añadir votos extras si es necesario
                            circuit.BlankVotes += random.Next(1, 20); // Genera un número aleatorio entre 1 y 50
                            circuit.NullVotes += random.Next(1, 20);
                            circuit.ObservedVotes += random.Next(1, 20);

                            circuit.Step1completed = true;
                            circuit.Step2completed = true;
                            circuit.Step3completed = true;
                        }

                        context.SaveChanges();
                        Console.WriteLine("Votos actualizados correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("No hay circuitos para actualizar.");
                    }
                }
            }
        }

        public static async Task<string> GetLatLongFromAddress(string address)
        {
            // Separar la dirección original en partes para poder procesarla
            var originalAddressParts = address.Split(new[] { "esq." }, StringSplitOptions.None);
            var streetNameWithNumber = originalAddressParts[0].Trim();
            var cornerStreetName = originalAddressParts.Length > 1 ? originalAddressParts[1].Trim() : string.Empty;
            var formattedAddress = $"{streetNameWithNumber} y {cornerStreetName}";

            string latLong = await AttemptGetLatLong(formattedAddress); // Primer intento con la dirección formateada (esquina)

            if (latLong == null && cornerStreetName != string.Empty)
            {
                // Intenta nuevamente con solo la calle y el número si la esquina falla
                latLong = await AttemptGetLatLong(streetNameWithNumber); // Segundo intento con la calle y el número
            }
            return latLong; // Puede ser nulo si ambos intentos fallan
        }

        private static async Task<string> AttemptGetLatLong(string queryAddress)
        {
            string baseUrl = "https://nominatim.openstreetmap.org/search?format=json&limit=1&q=";
            string fullUrl = $"{baseUrl}{Uri.EscapeDataString(queryAddress)}&countrycodes=UY";
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Datalexion_bulk");
                var response = await client.GetStringAsync(fullUrl);
                await Task.Delay(1000); // Añadir un retraso de 1 segundo entre llamadas para cumplir con la política de uso de la API
                var jsonArray = JArray.Parse(response);

                if (jsonArray.Count > 0)
                {
                    var lat = jsonArray[0]["lat"].ToString();
                    var lon = jsonArray[0]["lon"].ToString();
                    return $"{lat}, {lon}";
                }
            }
            return null; // No se encontraron resultados
        }

    }
}
