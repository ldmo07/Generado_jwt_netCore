using System.DirectoryServices;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace GENERADOR_JWT_UNIMINUTO.Helper
{
    public class DirectorioActivoHelper : IDirectorioActivoHelper
    {
        #region VARIABLES
        private readonly IConfiguration _configuration;
        #endregion

        #region CONSTRUCTOR
        public DirectorioActivoHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region LOGIN HACIA EL DA
        /// <summary>
        /// METODO ENCARGADO DE LOGEARSE EN EL DIRECTORIO ACTIVO
        /// </summary>
        /// <param name="email">CORREO</param>
        /// <param name="password">CONTRASEÑA</param>
        /// <returns>TRUE SI SE LOGEA FALSE SINO</returns>
        public bool loginDa(string email, string password)
        {
            string[] splitCorreo = email.Split('@');
            string dominio = splitCorreo[1];
            string correo = splitCorreo[0];

            string path = _configuration.GetSection("FullPathDA").Value!;

            //Armamos la cadena completa de dominio y usuario
            string domainAndUsername = dominio + @"\" + correo;
            //Creamos un objeto DirectoryEntry al cual le pasamos el URL, dominio/usuario y la contraseña
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, password);
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                //Verificamos que los datos de logeo proporcionados son correctos
                SearchResult result = search.FindOne();
                if (result == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
