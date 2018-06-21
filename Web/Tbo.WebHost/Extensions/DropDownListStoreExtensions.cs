using System.Collections.Generic;
using System.Web.Mvc;

namespace Tbo.WebHost.Extensions
{
    public static class DropDownListStoreExtensions
    {
        public static List<SelectListItem> AddEmptyElement(this List<SelectListItem> store)
        {
            return store.AddEmptyElement("Не выбран");
        }

        public static List<SelectListItem> AddEmptyElement(this List<SelectListItem> store, string text)
        {
            var emptyElement = new SelectListItem { Text = text, Value = "0" };
            store.Insert(0, emptyElement);
            return store;
        }
    }
}