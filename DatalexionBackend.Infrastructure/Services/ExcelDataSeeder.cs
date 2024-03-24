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
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ContextDB>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    // La base de datos necesita migraciones, probablemente no se ha inicializado.
                    // Esto puede ser un buen momento para ejecutar el seeding si solo quieres hacerlo una vez.
                    await LoadDataFromExcel(context);
                }
            }
        }

        private static async Task LoadDataFromExcel(ContextDB context)
        {
            var workbook = new XLWorkbook("Circuitos_upload.xlsx");
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed();

            var headers = worksheet.Row(1).CellsUsed().Select(c => c.Value.ToString()).ToList();

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

            // Agregando circuitos y asociando CircuitSlates y a CircuitParties
            foreach (var row in rows.Skip(1))
            {
                int circuitNumber = int.TryParse(row.Cell(numberIndex).Value.ToString().Trim(), out int result) ? result : 0;
                var circuitName = row.Cell(nameIndex).Value.ToString().Trim();
                var municipalityName = row.Cell(municipalityIndex).Value.ToString().Trim();
                var address = row.Cell(addressIndex).Value.ToString().Trim();
                string latLong = await GetLatLongFromAddress(address);

                if (municipalityIds.ContainsKey(municipalityName))
                {
                    var circuit = new Circuit
                    {
                        Name = circuitName,
                        Number = circuitNumber,
                        Address = address,
                        LatLong = latLong,
                        MunicipalityId = municipalityIds[municipalityName],
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
                    await context.SaveChangesAsync();
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
