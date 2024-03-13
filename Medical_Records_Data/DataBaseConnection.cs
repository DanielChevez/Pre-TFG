using Medical_Record_Models.ModelsGeneric;
using Medical_Record_Models.Results_Procedures;
using Medical_Record_Models.Results_Procedures.Folder;
using Medical_Record_Models.Tables;
using Medical_Record_Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Medical_Record_Data
{
    public class DataBaseConnection : DbContext
    {
        public DataBaseConnection(DbContextOptions<DataBaseConnection> options)
            : base(options)
        {

        }

        //TABLAS 
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Acciones> Acciones { get; set; }
        public DbSet<Parentesco> Parentesco { get; set; }




        //Expedientes
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Paciente> Paciente { get; set; }


        public DbSet<Expediente> Expediente { get; set; }
        public DbSet<Enfermedad> Enfermedad { get; set; }
        public DbSet<Cirugia> Cirugia { get; set; }
        public DbSet<Condicion_Medica> Condicion_Medica { get; set; }
        public DbSet<Habito> Habito { get; set; }

        public DbSet<Habito_No_Patologico> Habito_No_Patologico { get; set; }       

        // RESULTS PROCEDURES OF PRIVILEGES
        public DbSet<Arbol> Arbol { get; set; }
        public DbSet<genericModel> GenericModels { get; set; }
        public DbSet<FindUser> FindUser { get; set; }
        public DbSet<OpcionesusuariosPerfil> optionsUsersProfile { get; set; }
        public DbSet<OpcionesPerfilesDelUsuario> optionsProfilesUser { get; set; }

        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }
        public DbSet<Perfil_Usuario> PerfilUsuario { get; set; }



        //PROCEDURES OF FOLDER 


        public DbSet<Show_Folders_Results> Show_Folders_Results { get; set; }
        public DbSet<Show_Personal_Information> Show_Personal_Information { get; set; }
        public DbSet<Show_Family_Info> Show_Family_Info { get; set; }


        public DbSet<DiseaseCheckList> DiseaseCheckLists { get; set; }
        public DbSet<Antecedente> Antecedente { get; set; }

    }
}
