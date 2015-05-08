using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ENetCareMVC.Web
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }
        public string Path { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (Path != controllerContext.HttpContext.Request.Path)
                return false;
            
            string buttonValue = controllerContext.HttpContext.Request[MatchFormKey];
            string firstPartButtonValue = string.Empty;
            if (!string.IsNullOrEmpty(buttonValue))
            {
                var splits = buttonValue.Replace(" Id: ", " ").Split(' ');
                firstPartButtonValue = splits[0];
            }
            return firstPartButtonValue == MatchFormValue;
        }
    }
}