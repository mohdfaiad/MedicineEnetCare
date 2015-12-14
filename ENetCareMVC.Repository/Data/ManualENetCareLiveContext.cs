using System.Data.Entity;

namespace ENetCareMVC.Repository.Data
{
    public partial class Entities : DbContext
    {
        public Entities(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}
