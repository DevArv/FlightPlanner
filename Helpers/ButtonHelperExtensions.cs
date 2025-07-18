using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlightPlanner.Helpers;

public static class ButtonHelperExtensions
{
    public static IHtmlContent FpSaveButton(this IHtmlHelper HTMLHelper, string ID = "btnSave", string OnClick = "")
    {
        var button = new TagBuilder(tagName: "button");
        button.Attributes["type"] = "button";
        button.Attributes["id"] = ID;

        if (!string.IsNullOrWhiteSpace(OnClick))
        {
            button.Attributes["onclick"] = OnClick;
        }
        
        button.AddCssClass("btn btn-success");
        button.InnerHtml.Append("Guardar");
        return button;
    }
    
    public static IHtmlContent FpListButton(this IHtmlHelper HTMLHelper, string Page = "/Index")
    {
        var anchor = new TagBuilder(tagName: "a");
        anchor.Attributes["href"] = Page;
        anchor.AddCssClass("btn btn-primary");
        anchor.InnerHtml.Append("Listado");
        return anchor;
    }
    
    public static IHtmlContent FpDeleteButton(this IHtmlHelper HTMLHelper, string ID = "btnDelete", string OnClick = "")
    {
        var button = new TagBuilder(tagName: "button");
        button.Attributes["type"] = "button";
        button.Attributes["id"] = ID;

        if (!string.IsNullOrWhiteSpace(OnClick))
        {
            button.Attributes["onclick"] = OnClick;
        }
        
        button.AddCssClass("btn btn-danger");
        button.InnerHtml.Append("Eliminar");
        return button;
    }
    
    public static IHtmlContent FpEditButton(this IHtmlHelper HTMLHelper, string ID = "btnEdit", string OnClick = "")
    {
        var button = new TagBuilder(tagName: "button");
        button.Attributes["type"] = "button";
        button.Attributes["id"] = ID;

        if (!string.IsNullOrWhiteSpace(OnClick))
        {
            button.Attributes["onclick"] = OnClick;
        }
        
        button.AddCssClass("btn btn-primary");
        button.InnerHtml.Append("Editar");
        return button;
    }
}
