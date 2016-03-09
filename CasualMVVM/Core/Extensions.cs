using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuchsiaSoft.CasualMVVM.Core
{
    internal static class Extensions
    {
        /// <summary>
        /// Returns a list of all Entity Types for the current
        /// DbContext
        /// </summary>
        /// <param name="db">The context to interrogate</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetEntityTypes(this DbContext db)
        {
            List<Type> types = new List<Type>();

            ObjectItemCollection items =
                ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace
                .GetItemCollection(DataSpace.OSpace) as ObjectItemCollection;

            foreach (EntityType item in items.GetItems<EntityType>())
            {
                types.Add(items.GetClrType(item));
            }

            return types;
        }
    }
}
