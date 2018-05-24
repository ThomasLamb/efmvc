﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFinance.Domain;
namespace MyFinance.Helpers
{
    public static class ToSelectListItemsHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Category> categories, int  selectedId)
        {
            return

                categories.OrderBy(category => category.Name)
                      .Select(category =>
                          new SelectListItem
                          {
                              Selected = (category.CategoryId == selectedId),
                              Text = category.Name,
                              Value = category.CategoryId.ToString()
                          });
        }
    }
}