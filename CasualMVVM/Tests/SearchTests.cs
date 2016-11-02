using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaper.WindowMediation.WindowCreation;

namespace Vaper.Tests
{
    [TestClass]
    public class SearchTests
    {

        /// <summary>
        /// Used only as a fake for these tests to ensure
        /// reference type searching is functioning correctly
        /// </summary>
        private class ParentObject
        {
            [Searchable]
            public int Property1 { get; set; } = 12;

            [Searchable(displayPath: "Property1")]
            public ChildObject Property2 { get; set; } = new ChildObject()
            {
                Property1 = 42
            };
        }

        private class ChildObject
        {
            public int Property1 { get; set; } = 42;
        }

        /// <summary>
        /// Tests to make sure that reference type properties
        /// decorated with the <see cref="Searchable"/> attribute
        /// will search correctly in the <see cref="SearchViewModel{T}"/>
        /// e.g. MyObject.ComplexProperty.Name with Name being defined
        /// in the DisplayMemberPath of the attribute when applied
        /// to MyObject.ComplexProperty
        /// </summary>
        [TestMethod]
        public void EnsureSearchOnReferenceTypeProperties()
        {
            SearchViewModel<ParentObject> viewModel = new SearchViewModel<ParentObject>
                (new List<ParentObject>()
                {
                    new ParentObject(), //default instance, Property2.Property1 will be 42
                    new ParentObject()  //custom instance, Property2.Property1 won't be 42
                    {
                        Property2 = new ChildObject()
                        {
                            Property1 = 123
                        }
                    }
                });

            viewModel.FilterText = "42";
            viewModel.SearchCommand.Execute(null);
            Assert.IsTrue(viewModel.FilteredObjects.Count == 1);
        }
    }
}
