using System.Collections.Generic;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// A view model for a menu
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        /// <summary>
        /// The items in this menu
        /// </summary>
        public List<MenuItemViewModel> Items { get; set; }
    }
}
