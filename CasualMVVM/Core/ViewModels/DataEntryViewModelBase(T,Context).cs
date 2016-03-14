using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vaper.Core.ViewModels
{
    public abstract class DataEntryViewModelBase<T, Context> : 
        DataEntryViewModelBase, IDataEntryViewModel<T, Context>
        where T : class
        where Context : DbContext
    {
        protected Context _DbContext;

        protected DataEntryViewModelBase(T entity, DataEntryMode mode)
        {
            _DbContext = Activator.CreateInstance<Context>();

            _Entity = entity;

            Mode = mode;

            ValidateState();

            Initialise();
        }

        protected virtual void Initialise()
        {
            _DbContext.Set<T>().Attach(_Entity);

            Load();
        }

        private const string NOT_ENTITY_MESSAGE =
            "The type specified for the generic DataEntryViewModelBase<T, Context> is " +
            "not an entity type known to the DbContext Type specified.  Make sure that the " +
            "context and object type you are using are both from the same entity model.";

        protected T _Entity;

        private IEnumerable<Type> _EntityTypes = GetEntityTypes();

        private static IEnumerable<Type> GetEntityTypes()
        {
            using (Context db = (Activator.CreateInstance<Context>()))
            {
                return db.GetEntityTypes();
            }
        }

        /// <summary>
        /// Performs a quick check to make sure that the provided entity T
        /// matches a known Entity Type in supplied Context Type
        /// </summary>
        private void ValidateState()
        {
            if (!_EntityTypes.Any(t => t == typeof(T)))
            {
                throw new NotSupportedException(NOT_ENTITY_MESSAGE);
            }
        }

        //TODO: temporary, get rid of this bit
        //public abstract void Load(Context db);
        public abstract void Load();

        /// <summary>
        /// Will call either SaveNew or SaveExisting
        /// depending on the Mode property of the
        /// derived viewmodel.  It will also check for
        /// database conflicts and if found 
        /// <see cref="ContinueOnConflict(IEnumerable{PropertyChangeInfo})">
        /// </see> to determine whether or not to actually
        /// call Save or SaveNew.
        /// Finally it will dispose of the DbContext held
        /// by the derived viewmodel, and this method is 
        /// sealed to make sure that it will always be handled
        /// correctly.
        /// </summary>
        /// <param name="parameter"></param>
        public override sealed void Save(object parameter)
        {
            IEnumerable<PropertyChangeInfo> conflicts = GetConflicts(); 
            if(conflicts.Count() != 0)
            {
                if (!ContinueOnConflict(conflicts))
                {
                    return;
                }
                else
                {
                    _DbContext.Entry<T>(_Entity).State = EntityState.Modified;
                }
            }

            base.Save(parameter);

            _DbContext.Dispose();
        }

        /// <summary>
        /// Calls <see cref="DataEntryViewModelBase.Cancel"/>
        /// but also disposes of the viewmodel's context.
        /// Therefore sealed to make sure that it will
        /// always be handled correctly.
        /// </summary>
        protected override sealed void Cancel()
        {
            base.Cancel();

            _DbContext.Dispose();
        }

        protected override void _ActiveWindow_Closed(object sender, EventArgs e)
        {
            base._ActiveWindow_Closed(sender, e);
            _DbContext.Dispose();
        }

        private IEnumerable<PropertyChangeInfo> GetConflicts()
        {
            List<PropertyChangeInfo> conflicts = new List<PropertyChangeInfo>();

            if (_DbContext.Entry<T>(_Entity).GetDatabaseValues() == null)
            {
                //entity doesn't yet exist in database
                //so no conflicts possible
                return conflicts;
            }

            T database = _DbContext.Entry<T>(_Entity).GetDatabaseValues().ToObject() as T;
            T original = _DbContext.Entry<T>(_Entity).OriginalValues.ToObject() as T;

            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            for (int i = 0; i < properties.Count; i++)
            {
                if (!Object.Equals(properties[i].GetValue(original),
                    properties[i].GetValue(database)))
                {
                    conflicts.Add(new PropertyChangeInfo()
                    {
                        PropertyInfo = properties[i],
                        OriginalValue = properties[i].GetValue(original),
                        DatabaseValue = properties[i].GetValue(database),
                        ChangedValue = properties[i].GetValue(_Entity)
                    });
                }
            }

            return conflicts;
        }


        protected override bool SaveExisting(object parameter)
        {
            _DbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Saves the <see cref="T"/> to the database as a new record.  This
        /// method will also attempt to attach any related entities if they
        /// already exist in the database, and create them if not.
        /// </summary>
        /// <param name="parameter"></param>
        protected override bool SaveNew(object parameter)
        {
            AttachRelatedProperties(_DbContext);

            _DbContext.Set<T>().Add(_Entity);

            _DbContext.SaveChanges();

            return true;
        }


        private void AttachRelatedProperties(Context db)
        {
            foreach (PropertyInfo property in _Entity.GetType().GetProperties())
            {
                if (IsEntityType(property))
                {
                    object entity = property.GetValue(_Entity);
                    DbSet set = db.Set(property.PropertyType);

                    set.Attach(entity);

                    ObjectContext context = ((IObjectContextAdapter)db).ObjectContext;


                    MarkAsModified(db, entity);

                    if (db.Entry(entity).GetDatabaseValues() == null)
                    {
                        set.Add(entity);
                    }
                }
            }
        }


        /// <summary>
        /// Marks the specified entity as modified within the specified context
        /// just an internal helper method to get rid of a repeating, and ugly block
        /// of code.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        private void MarkAsModified(Context db, object entity)
        {
            ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager
                    .ChangeObjectState(entity, EntityState.Modified);
        }


        /// <summary>
        /// Checks if the provided property is a type included
        /// in the Entity Model, and if so returns true
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsEntityType(PropertyInfo property)
        {
            if (_EntityTypes.Any(t => t == property.PropertyType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Override in a derived viewmodel to determine whether
        /// the save operations should continue if a conflict
        /// is identified.  This method could be used to
        /// inform the user that a conflict exists and 
        /// determine from their input whether or not the
        /// operation should continue.
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public virtual bool ContinueOnConflict(IEnumerable<PropertyChangeInfo> conflicts)
        {
            return true;
        }
    }
}
