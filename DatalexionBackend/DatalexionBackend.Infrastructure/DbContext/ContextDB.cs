using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatalexionBackend.Infrastructure.DbContext
{
    public class ContextDB : IdentityDbContext
    {
        private readonly string _baseUrl;

        public ContextDB(DbContextOptions<ContextDB> options, IConfiguration configuration) : base(options)
        {
            _baseUrl = configuration.GetValue<string>("Backend_URL");
        }

        #region DB Tables

        public DbSet<Log> Log { get; set; }
        public DbSet<DatalexionUser> DatalexionUser { get; set; }
        public DbSet<DatalexionRole> DatalexionRole { get; set; }
        public DbSet<Candidate> Candidate { get; set; }
        public DbSet<Circuit> Circuit { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Delegado> Delegado { get; set; }
        public DbSet<Municipality> Municipality { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<Party> Party { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Slate> Slate { get; set; }
        public DbSet<Wing> Wing { get; set; }

        // N-N
        public DbSet<CircuitDelegado> CircuitDelegado { get; set; }
        public DbSet<CircuitSlate> CircuitSlate { get; set; }
        public DbSet<CircuitParty> CircuitParty { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region modelbuilder

            modelBuilder.Entity<Circuit>()
                .HasOne(c => c.Municipality)
                .WithMany(m => m.ListCircuits)
                .HasForeignKey(c => c.MunicipalityId); // Configura la clave foránea explícitamente

            // Candidate -> Photos
            modelBuilder.Entity<Candidate>()
                .HasOne(p => p.Photo)
                .WithOne()
                .HasForeignKey<Candidate>(p => p.PhotoId);

            // Wing -> Photos
            modelBuilder.Entity<Wing>()
                .HasOne(p => p.Photo)
                .WithOne()
                .HasForeignKey<Wing>(p => p.PhotoId);

            // Slate -> Photos
            modelBuilder.Entity<Slate>()
                .HasOne(p => p.Photo)
                .WithOne()
                .HasForeignKey<Slate>(p => p.PhotoId);
            //

            // 1..N
            modelBuilder.Entity<Client>()
                .HasMany(c => c.ListUsers)
                .WithOne()
                .HasForeignKey(w => w.ClientId);

            // 1..N
            modelBuilder.Entity<Client>()
                .HasMany(c => c.ListDelegados)
                .WithOne()
                .HasForeignKey(w => w.ClientId);

            // 1..N
            modelBuilder.Entity<Party>()
                .HasMany(c => c.ListWings)
                .WithOne()
                .HasForeignKey(w => w.PartyId);

            // Party -> Photos 1
            modelBuilder.Entity<Party>()
                .HasOne(p => p.PhotoLong)
                .WithOne()
                .HasForeignKey<Party>(p => p.PhotoLongId);

            // Party -> Photos 2
            modelBuilder.Entity<Party>()
                .HasOne(p => p.PhotoShort)
                .WithOne()
                .HasForeignKey<Party>(p => p.PhotoShortId);
            //

            // 1..N
            modelBuilder.Entity<Municipality>()
                .HasMany(c => c.ListCircuits)
                .WithOne()
                .HasForeignKey(w => w.MunicipalityId);

            // 1..N
            modelBuilder.Entity<Province>()
                .HasMany(c => c.ListSlates)
                .WithOne()
                .HasForeignKey(w => w.ProvinceId);
            // 1..N
            modelBuilder.Entity<Province>()
                .HasMany(c => c.ListMunicipalities)
                .WithOne()
                .HasForeignKey(w => w.ProvinceId);

            // 1..N
            modelBuilder.Entity<Slate>()
                .HasMany(c => c.ListParticipants)
                .WithOne()
                .HasForeignKey(w => w.SlateId);

            // 1..N
            modelBuilder.Entity<Wing>()
                .HasMany(c => c.ListSlates)
                .WithOne()
                .HasForeignKey(w => w.WingId);

            // N..N
            modelBuilder.Entity<CircuitDelegado>()
                .HasKey(v => new { v.CircuitId, v.DelegadoId });

            // N..N
            modelBuilder.Entity<CircuitSlate>()
                .HasKey(v => new { v.CircuitId, v.SlateId });

            modelBuilder.Entity<CircuitSlate>()
                .HasOne(cs => cs.Circuit)
                .WithMany(c => c.ListCircuitSlates)
                .HasForeignKey(cs => cs.CircuitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CircuitSlate>()
                .HasOne(cs => cs.Slate)
                .WithMany(s => s.ListCircuitSlates)
                .HasForeignKey(cs => cs.SlateId)
                .OnDelete(DeleteBehavior.Restrict);

            // N..N
            modelBuilder.Entity<CircuitParty>()
                .HasKey(v => new { v.CircuitId, v.PartyId });

            modelBuilder.Entity<CircuitParty>()
                .HasOne(cs => cs.Circuit)
                .WithMany(c => c.ListCircuitParties)
                .HasForeignKey(cs => cs.CircuitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CircuitParty>()
                .HasOne(cs => cs.Party)
                .WithMany(s => s.ListCircuitParties)
                .HasForeignKey(cs => cs.PartyId)
                .OnDelete(DeleteBehavior.Restrict);

            #region Relaciones N - 1 (required) y vuelta 1 - N

            // relación N - 1 (required) y 1 - N 
            modelBuilder.Entity<Delegado>()
                .HasOne<Client>(d => d.Client)
                .WithMany(c => c.ListDelegados)
                .HasForeignKey(d => d.ClientId);

            // relación N - 1 (required) y 1 - N 
            modelBuilder.Entity<Slate>()
                .HasOne(s => s.Province)
                .WithMany(p => p.ListSlates)
                .HasForeignKey(s => s.ProvinceId);

            // relación N - 1 (required) y 1 - N 
            modelBuilder.Entity<Slate>()
                .HasOne(s => s.Wing)
                .WithMany(p => p.ListSlates)
                .HasForeignKey(s => s.WingId);

            // relación N - 1 (required) y 1 - N 
            modelBuilder.Entity<Wing>()
                .HasOne(s => s.Party)
                .WithMany(p => p.ListWings)
                .HasForeignKey(s => s.PartyId);

            // relación N - 1 (required) y 1 - N 
            modelBuilder.Entity<DatalexionUser>()
                .HasOne(s => s.Client)
                .WithMany(p => p.ListUsers)
                .HasForeignKey(s => s.ClientId);

            #endregion Relaciones N - 1 (required) y vuelta 1 - N

            // GPT Fixing

            modelBuilder.Entity<Municipality>()
             .HasOne(m => m.Province)
             .WithMany(p => p.ListMunicipalities)
             .HasForeignKey(m => m.ProvinceId)
             .OnDelete(DeleteBehavior.Restrict); // Evita la eliminación en cascada

            #endregion modelbuilder

            // La llamada a SeedUsers se mantiene aquí para cualquier seeding que siempre debe ejecutarse
            SeedUsers(modelBuilder);
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/20660148#notes
            // Generar GUID: https://guidgenerator.com/online-guid-generator.aspx
            // ---------------- Usuarios ---------------------------------------------
            var userAdminId = "c2ee6493-5a73-46f3-a3f2-46d1d11d7176";
            var rolAdminId = "bef4cbd4-1f2b-472f-a1e2-e1a901f6808c";

            var userAnalystId = "e0765c93-676c-4199-b7ee-d7877c471821";
            var rolAnalystId = "bef4cbd4-1f2b-472f-a3f2-e1a901f6811c";

            var userfaId = "2a765d8b-9204-4e0f-b4ce-453f6e1bb592";
            var userpnId = "8498a3ff-ca69-4b93-9a37-49a73c8dec77";
            var userpcId = "6c762a89-a7b6-4ee3-96d0-105b219dcaa6";

            var rolAdmin = new DatalexionRole
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                Creation = DateTime.Now,
                Update = DateTime.Now
            };

            var rolUser = new DatalexionRole
            {
                Id = rolAnalystId,
                Name = "Analyst",
                NormalizedName = "ANALYST",
                Creation = DateTime.Now,
                Update = DateTime.Now
            };

            var passwordHasher = new PasswordHasher<DatalexionUser>();

            var username1 = "useradmin";
            var email1 = "admin@datalexion.lat";
            var userAdmin = new DatalexionUser()
            {
                Id = userAdminId,
                UserName = username1,
                NormalizedUserName = username1.ToUpper(),
                Email = email1,
                NormalizedEmail = email1.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "useradmin1234"),
                Name = "Usuario administrador",
                ClientId = 2,
            };

            var adminfa1 = "adminfa";
            var emailfa = "adminfa@datalexion.lat";
            var adminfa = new DatalexionUser()
            {
                Id = userfaId,
                UserName = adminfa1,
                NormalizedUserName = adminfa1.ToUpper(),
                Email = emailfa,
                NormalizedEmail = emailfa.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "adminfa1234"),
                Name = "Admin FA",
                ClientId = 1,
            };

            var adminpn1 = "adminpn";
            var emailpn = "adminpn@datalexion.lat";
            var adminpn = new DatalexionUser()
            {
                Id = userpnId,
                UserName = adminpn1,
                NormalizedUserName = adminpn1.ToUpper(),
                Email = emailpn,
                NormalizedEmail = emailpn.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "adminpn1234"),
                Name = "Admin PN",
                ClientId = 2,
            };

            var adminpc1 = "adminpc";
            var emailpc = "adminpc@datalexion.lat";
            var adminpc = new DatalexionUser()
            {
                Id = userpcId,
                UserName = adminpc1,
                NormalizedUserName = adminpc1.ToUpper(),
                Email = emailpc,
                NormalizedEmail = emailpc.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "adminpc1234"),
                Name = "Admin PC",
                ClientId = 3,
            };

            var username2 = "useranalista";
            var email2 = "normal@datalexion.lat";
            var userUser = new DatalexionUser()
            {
                Id = userAnalystId,
                UserName = username2,
                NormalizedUserName = username2.ToUpper(),
                Email = email2,
                NormalizedEmail = email2.ToUpper(),
                PasswordHash = passwordHasher.HashPassword(null, "usernormal1234"),
                Name = "Usuario analista",
                ClientId = 2,
            };

            modelBuilder.Entity<DatalexionUser>()
                .HasData(userAdmin, userUser, adminfa, adminpn, adminpc);

            modelBuilder.Entity<DatalexionRole>()
                .HasData(rolAdmin, rolUser);

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 1,
                    ClaimType = "role",
                    UserId = userAdminId,
                    ClaimValue = "Admin"
                });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 2,
                    ClaimType = "role",
                    UserId = userAnalystId,
                    ClaimValue = "Analyst"
                });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 3,
                    ClaimType = "role",
                    UserId = userfaId,
                    ClaimValue = "Admin"
                });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 4,
                    ClaimType = "role",
                    UserId = userpnId,
                    ClaimValue = "Admin"
                });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 5,
                    ClaimType = "role",
                    UserId = userpcId,
                    ClaimValue = "Admin"
                });

            // Asignar roles a usuarios
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = rolAdminId,
                    UserId = userAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = rolAnalystId,
                    UserId = userAnalystId
                },
                new IdentityUserRole<string>
                {
                    RoleId = rolAdminId,
                    UserId = userfaId
                },
                new IdentityUserRole<string>
                {
                    RoleId = rolAdminId,
                    UserId = userpnId
                },
                new IdentityUserRole<string>
                {
                    RoleId = rolAdminId,
                    UserId = userpcId
                }
            );

            // Candidatos fotos

            // Lógica de Fotos:
            /*
                1-100:      Candidatos
                101-200:    Partidos
                101-200:    Agrupaciones
                201-300:    Listas
            */

            var fotosCandidatos = new List<Photo>
                {
                    new Photo { Id = 1, URL = $"{_baseUrl}/uploads/candidates/photo1.jpg" },
                    new Photo { Id = 2, URL = $"{_baseUrl}/uploads/candidates/photo1.jpg" },
                    new Photo { Id = 3, URL = $"{_baseUrl}/uploads/candidates/photo3.jpg" },
                    new Photo { Id = 4, URL = $"{_baseUrl}/uploads/candidates/photo4.jpg" },
                    new Photo { Id = 5, URL = $"{_baseUrl}/uploads/candidates/photo5.jpg" },
                    new Photo { Id = 6, URL = $"{_baseUrl}/uploads/candidates/photo6.jpg" },
                    new Photo { Id = 7, URL = $"{_baseUrl}/uploads/candidates/photo7.jpg" },
                    new Photo { Id = 8, URL = $"{_baseUrl}/uploads/candidates/photo8.jpg" },
                    new Photo { Id = 9, URL = $"{_baseUrl}/uploads/candidates/photo9.jpg" },
                    new Photo { Id = 10, URL = $"{_baseUrl}/uploads/candidates/photo10.jpg" },
                    new Photo { Id = 11, URL = $"{_baseUrl}/uploads/candidates/photo11.jpg" },
                    new Photo { Id = 12, URL = $"{_baseUrl}/uploads/candidates/photo12.jpg" },
                    new Photo { Id = 13, URL = $"{_baseUrl}/uploads/candidates/photo13.jpg" },
                    new Photo { Id = 14, URL = $"{_baseUrl}/uploads/candidates/photo14.jpg" },
                    new Photo { Id = 15, URL = $"{_baseUrl}/uploads/candidates/photo15.jpg" },
                    new Photo { Id = 16, URL = $"{_baseUrl}/uploads/candidates/photo16.jpg" },
                };
            modelBuilder.Entity<Photo>().HasData(fotosCandidatos.ToArray());

            #region Candidates

            // Candidatos 

            var candidate1 = new Candidate()
            {
                Id = 1,
                Name = "Álvaro Delgado",
                PhotoId = 1
            };

            var candidate2 = new Candidate()
            {
                Id = 2,
                Name = "Laura Raffo",
                PhotoId = 2
            };

            var candidate3 = new Candidate()
            {
                Id = 3,
                Name = "Jorge Gandini",
                PhotoId = 3
            };

            var candidate4 = new Candidate()
            {
                Id = 4,
                Name = "Juan Sartori",
                PhotoId = 4
            };
            var candidate5 = new Candidate()
            {
                Id = 5,
                Name = "Yamandú Orsi",
                PhotoId = 5
            };

            var candidate6 = new Candidate()
            {
                Id = 6,
                Name = "Carolina Cosse",
                PhotoId = 6
            };
            var candidate7 = new Candidate()
            {
                Id = 7,
                Name = "Mario Bergara",
                PhotoId = 7
            };
            var candidate8 = new Candidate()
            {
                Id = 8,
                Name = "Pablo Mieres",
                PhotoId = 8
            };
            var candidate9 = new Candidate()
            {
                Id = 9,
                Name = "Edgardo Novick",
                PhotoId = 9
            };
            var candidate10 = new Candidate()
            {
                Id = 10,
                Name = "Andrés Lima",
                PhotoId = 10
            };
            var candidate11 = new Candidate()
            {
                Id = 11,
                Name = "Gabriel Gurméndez",
                PhotoId = 11
            };
            var candidate12 = new Candidate()
            {
                Id = 12,
                Name = "Robert Silva",
                PhotoId = 12
            };
            var candidate13 = new Candidate()
            {
                Id = 13,
                Name = "Andrés Ojeda",
                PhotoId = 13
            };
            var candidate14 = new Candidate()
            {
                Id = 14,
                Name = "Gustavo Zubía",
                PhotoId = 14
            };
            var candidate15 = new Candidate()
            {
                Id = 15,
                Name = "Guzmán Acosta y Lara",
                PhotoId = 15
            };
            var candidate16 = new Candidate()
            {
                Id = 16,
                Name = "Tabaré Viera",
                PhotoId = 16
            };

            modelBuilder.Entity<Candidate>()
                .HasData(candidate1, candidate2, candidate3, candidate4, candidate5, candidate6, candidate7, candidate8, candidate9, candidate10, candidate11, candidate12, candidate13, candidate14, candidate15, candidate16);

            #endregion Candidates

            #region Slates

            // Listas
            // FA
            var slate1 = new Slate()
            {
                Id = 1,
                Name = "5005",
                Color = "#3153dd",
                WingId = 1,
                CandidateId = 6,
                ProvinceId = 1
            };

            var slate2 = new Slate()
            {
                Id = 2,
                Name = "711",
                Color = "#3153dd",
                WingId = 1,
                CandidateId = 5,
                ProvinceId = 1
            };

            var slate3 = new Slate()
            {
                Id = 3,
                Name = "90",
                Color = "#d62929",
                WingId = 1,
                CandidateId = 6,
                ProvinceId = 1
            };

            var slate4 = new Slate()
            {
                Id = 4,
                Name = "609",
                Color = "#b929d6",
                WingId = 1,
                CandidateId = 5,
                ProvinceId = 1
            };

            var slate5 = new Slate()
            {
                Id = 5,
                Name = "71",
                Color = "#bec11a",
                WingId = 2,
                CandidateId = 1,
                ProvinceId = 1
            };

            var slate6 = new Slate()
            {
                Id = 6,
                Name = "404",
                Color = "#3153dd",
                WingId = 2,
                CandidateId = 1,
                ProvinceId = 1
            };

            var slate7 = new Slate()
            {
                Id = 7,
                Name = "40",
                Color = "#ff0000",
                WingId = 2,
                CandidateId = 2,
                ProvinceId = 1
            };

            var slate8 = new Slate()
            {
                Id = 8,
                Name = "250",
                Color = "#00ff00",
                WingId = 2,
                CandidateId = 2,
                ProvinceId = 1
            };

            var slate9 = new Slate()
            {
                Id = 9,
                Name = "880",
                Color = "#0000ff",
                WingId = 2,
                CandidateId = 2,
                ProvinceId = 1
            };

            var slate10 = new Slate()
            {
                Id = 10,
                Name = "15",
                Color = "#ff00ff",
                WingId = 3,
                CandidateId = 11,
                ProvinceId = 1
            };

            var slate11 = new Slate()
            {
                Id = 11,
                Name = "85",
                Color = "#987654",
                WingId = 3,
                CandidateId = 12,
                ProvinceId = 1
            };

            var slate12 = new Slate()
            {
                Id = 12,
                Name = "1",
                Color = "#abcdef",
                WingId = 3,
                CandidateId = 12,
                ProvinceId = 1
            };

            var slate13 = new Slate()
            {
                Id = 13,
                Name = "2000",
                Color = "#fedcba",
                WingId = 3,
                CandidateId = 12,
                ProvinceId = 1
            };

            var slate14 = new Slate()
            {
                Id = 14,
                Name = "1515",
                Color = "#012345",
                WingId = 3,
                CandidateId = 11,
                ProvinceId = 1
            };
            var slate15 = new Slate()
            {
                Id = 15,
                Name = "600",
                Color = "#012345",
                WingId = 3,
                CandidateId = 11,
                ProvinceId = 1
            };

            modelBuilder.Entity<Slate>()
                    .HasData(slate1, slate2, slate3, slate4, slate5, slate6, slate7, slate8, slate9, slate10, slate11, slate12, slate13, slate14, slate15);

            #endregion Slates

            // Partidos fotos

            // Lógica de Fotos:
            /*
                1-100:      Candidatos
                101-200:    Partidos
                101-200:    Agrupaciones
                201-300:    Listas
            */

            var fotosPartidos = new List<Photo>
                {
                    // long 101
                    new Photo { Id = 101, URL = $"{_baseUrl}/uploads/partiesLong/photo101.jpg" },
                    new Photo { Id = 102, URL = $"{_baseUrl}/uploads/partiesLong/photo102.jpg" },
                    new Photo { Id = 103, URL = $"{_baseUrl}/uploads/partiesLong/photo103.jpg" },
                    new Photo { Id = 104, URL = $"{_baseUrl}/uploads/partiesLong/photo104.jpg" },
                    new Photo { Id = 105, URL = $"{_baseUrl}/uploads/partiesLong/photo105.jpg" },
                    new Photo { Id = 106, URL = $"{_baseUrl}/uploads/partiesLong/photo106.jpg" },

                    // short 111
                    new Photo { Id = 111, URL = $"{_baseUrl}/uploads/partiesShort/photo111.png" },
                    new Photo { Id = 112, URL = $"{_baseUrl}/uploads/partiesShort/photo112.png" },
                    new Photo { Id = 113, URL = $"{_baseUrl}/uploads/partiesShort/photo113.png" },
                    new Photo { Id = 114, URL = $"{_baseUrl}/uploads/partiesShort/photo114.png" },
                    new Photo { Id = 115, URL = $"{_baseUrl}/uploads/partiesShort/photo115.png" },
                    new Photo { Id = 116, URL = $"{_baseUrl}/uploads/partiesShort/photo116.png" },
                    };
            modelBuilder.Entity<Photo>().HasData(fotosPartidos.ToArray());

            #region Parties

            // Partidos

            var party1 = new Party()
            {
                Id = 1,
                Name = "Frente Amplio",
                ShortName = "FA",
                Color = "#3153dd",
                PhotoLongId = 101,
                PhotoShortId = 111
            };
            var party2 = new Party()
            {
                Id = 2,
                Name = "Partido Nacional",
                ShortName = "PN",
                Color = "#3153dd",
                PhotoLongId = 102,
                PhotoShortId = 112

                //
            };
            var party3 = new Party()
            {
                Id = 3,
                Name = "Partido Colorado",
                ShortName = "PC",
                Color = "#d62929",
                PhotoLongId = 103,
                PhotoShortId = 113
            };
            var party4 = new Party()
            {
                Id = 4,
                Name = "Partido Independiente",
                ShortName = "PI",
                Color = "#b929d6",
                PhotoLongId = 104,
                PhotoShortId = 114
            };
            var party5 = new Party()
            {
                Id = 5,
                Name = "Cabildo Abierto",
                ShortName = "CA",
                Color = "#f9bb28",
                PhotoLongId = 105,
                PhotoShortId = 115
            };
            var party6 = new Party()
            {
                Id = 6,
                Name = "PERI",
                ShortName = "PERI",
                Color = "#009001",
                PhotoLongId = 106,
                PhotoShortId = 116
            };

            modelBuilder.Entity<Party>()
                                .HasData(party1, party2, party3, party4, party5, party6);

            #endregion Parties

            #region Clients

            // Clientes partidos
            var clientFA = new Client()
            {
                Id = 1,
                Name = "Frente Amplio",
                Comments = "Frente Amplio",
                Creation = DateTime.Now,
                Update = DateTime.Now,
                PartyId = 1,
            };

            var clientPN = new Client()
            {
                Id = 2,
                Name = "Partido Nacional",
                Comments = "Partido Nacional",
                Creation = DateTime.Now,
                Update = DateTime.Now,
                PartyId = 2,
            };

            var clientPC = new Client()
            {
                Id = 3,
                Name = "Partido Colorado",
                Comments = "Partido Colorado",
                Creation = DateTime.Now,
                Update = DateTime.Now,
                PartyId = 3,
            };

            modelBuilder.Entity<Client>().HasData(clientFA, clientPN, clientPC);

            #endregion Clients

            #region Delegados

            var delegadoCI = "11111111";
            var delegadoEmail = "gonzalo.delegado@datalexion.lat";
            var delegadoId = 1;
            var delegadoPN1 = new Delegado()
            {
                Id = delegadoId,
                CI = delegadoCI,
                Name = "Gonzalo",
                Email = delegadoEmail,
                Phone = "099415831",
                ClientId = clientPN.Id
            };

            delegadoCI = "12345678";
            delegadoEmail = "diego.delegado@datalexion.lat";
            delegadoId = 2;
            var delegadoPN2 = new Delegado()
            {
                Id = delegadoId,
                CI = delegadoCI,
                Name = "Diego",
                Email = delegadoEmail,
                Phone = "099589896",
                ClientId = clientPN.Id
            };

            delegadoCI = "22222222";
            delegadoEmail = "pablo.delegado@datalexion.lat";
            delegadoId = 3;
            var delegadoFA = new Delegado()
            {
                Id = delegadoId,
                CI = delegadoCI,
                Name = "Pablo",
                Email = delegadoEmail,
                Phone = "099415831",
                ClientId = clientFA.Id
            };

            delegadoCI = "33333333";
            delegadoEmail = "gabriel.delegado@datalexion.lat";
            delegadoId = 4;
            var delegadoPC = new Delegado()
            {
                Id = delegadoId,
                CI = delegadoCI,
                Name = "Gabriel",
                Email = delegadoEmail,
                Phone = "099415831",
                ClientId = clientPC.Id
            };

            modelBuilder.Entity<Delegado>()
                .HasData(delegadoPN1, delegadoPN2, delegadoFA, delegadoPC);

            #endregion Delegados

            #region Wings

            // Agrupaciones - Wings

            var wing1 = new Wing()
            {
                Id = 1,
                Name = "FA",
                PartyId = 1
            };
            var wing2 = new Wing()
            {
                Id = 2,
                Name = "PN",
                PartyId = 2
            };
            var wing3 = new Wing()
            {
                Id = 3,
                Name = "PC",
                PartyId = 3
            };
            var wing4 = new Wing()
            {
                Id = 4,
                Name = "PI",
                PartyId = 4
            };
            var wing5 = new Wing()
            {
                Id = 5,
                Name = "CA",
                PartyId = 5
            };
            var wing6 = new Wing()
            {
                Id = 6,
                Name = "PERI",
                PartyId = 6
            };

            modelBuilder.Entity<Wing>().HasData(wing1, wing2, wing3, wing4, wing5, wing6);

            #endregion Wings

            #region Provinces

            var province1 = new Province()
            {
                Id = 1,
                Name = "Montevideo",
            };
            var province2 = new Province()
            {
                Id = 2,
                Name = "Canelones",
            };
            var province3 = new Province()
            {
                Id = 3,
                Name = "Maldonado",
            };
            var province4 = new Province()
            {
                Id = 4,
                Name = "Rocha",
            };
            var province5 = new Province()
            {
                Id = 5,
                Name = "Colonia",
            };
            var province6 = new Province()
            {
                Id = 6,
                Name = "Artigas",
            };
            var province7 = new Province()
            {
                Id = 7,
                Name = "Salto",
            };
            var province8 = new Province()
            {
                Id = 8,
                Name = "Paysandú",
            };
            var province9 = new Province()
            {
                Id = 9,
                Name = "Tacuarembó",
            };
            var province10 = new Province()
            {
                Id = 10,
                Name = "Rivera",
            };
            var province11 = new Province()
            {
                Id = 11,
                Name = "San José",
            };
            var province12 = new Province()
            {
                Id = 12,
                Name = "Durazno",
            };
            var province13 = new Province()
            {
                Id = 13,
                Name = "Treinta y Tres",
            };
            var province14 = new Province()
            {
                Id = 14,
                Name = "Cerro Largo",
            };
            var province15 = new Province()
            {
                Id = 15,
                Name = "Rivera",
            };
            var province16 = new Province()
            {
                Id = 16,
                Name = "Flores",
            };
            var province17 = new Province()
            {
                Id = 17,
                Name = "Florida",
            };
            var province18 = new Province()
            {
                Id = 18,
                Name = "Lavalleja",
            };
            var province19 = new Province()
            {
                Id = 19,
                Name = "Soriano",
            };

            modelBuilder.Entity<Province>()
                            .HasData(province1, province2, province3, province4, province5, province6, province7, province8, province9, province10, province11, province12, province13, province14, province15, province16, province17, province18, province19);

            #endregion Provinces

        }
    }
}