using AwesomeDevEventsAPI.Etities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEventsAPI.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
            
        }
        //DbSet = tabelas
        public DbSet<DevEvent> DevEvents {  get; set; }
        public DbSet<DevEventSpeaker> DevEventSpeakers { get; set; }

        //Configurações de model(api) para Entidade(banco)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//e--> entidade de-->devEvent mas poderia ser quaisquer nomes

            modelBuilder.Entity<DevEvent>(e=>
            { //Configuração para chaves primarias
             //Sem colocar nenhuma propriedade, ele segue a convenção. Como aqui:
                e.HasKey(de => de.Id);

                //Configuração de propriedades
                //IsRequired= não aceita valores nulos
                e.Property(de => de.Title).IsRequired();

                //MaxLength= define o tamanho da coluna 
                //ColumnType= define o tipo de dados
                e.Property(de => de.Description).IsRequired(false).HasMaxLength(200).HasColumnType("varchar(200)");

                //Column name=Define um nome específico para aquela coluna
                e.Property(de => de.StartDate).HasColumnName("Start_date");

                e.Property(de => de.EndDate).HasColumnName("End_date");

                //Configuração da relação entre tabelas
                // s-->speakers
                e.HasMany(de => de.Speakers)
                .WithOne()
                .HasForeignKey(s => s.DevEventId);

            });



            //Configuração para chaves primarias
            modelBuilder.Entity<DevEventSpeaker>(e => { e.HasKey(de => de.Id); });
        }
    }
}
