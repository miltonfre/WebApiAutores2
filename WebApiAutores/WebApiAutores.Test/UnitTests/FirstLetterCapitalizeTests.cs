using System.ComponentModel.DataAnnotations;
using WebApiAutores.CustomValidators;

namespace WebApiAutores.Test.UnitTests
{
    [TestClass]
    public class FirstLetterCapitalizeTests
    {
        [TestMethod]
        public void FirstLetterLowerCaseReturnError()
        {
            //Prepare
            var firstLetterCapitalize = new FirstLetterCapitalize();
            var value = "vanessa";
            var valContext = new ValidationContext(new { Name = value });

            //execution
            var result = firstLetterCapitalize.GetValidationResult(value, valContext);

            //verify
            Assert.AreEqual("La primera letra debe ser mayuscula", result.ErrorMessage);
        }

        [TestMethod]
        public void FirstLetterNullReturnOk()
        {
            //Prepare
            var firstLetterCapitalize = new FirstLetterCapitalize();
            string value = null;
            var valContext = new ValidationContext(new { Name = value });

            //execution
            var result = firstLetterCapitalize.GetValidationResult(value, valContext);

            //verify
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FirstLetterUpperCaseReturnOk()
        {
            //Prepare
            var firstLetterCapitalize = new FirstLetterCapitalize();
            var value = "Vanessa";
            var valContext = new ValidationContext(new { Name = value });

            //execution
            var result = firstLetterCapitalize.GetValidationResult(value, valContext);

            //verify
            Assert.IsNull(result);
        }
    }
}