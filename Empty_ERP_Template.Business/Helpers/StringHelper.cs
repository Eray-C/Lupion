using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empty_ERP_Template.Business.Helpers;

public class StringHelper
{
    public static string NormalizePath(string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < segments.Length; i++)
            if (segments[i].All(char.IsDigit))
                segments[i] = "{id}";
        return "/" + string.Join("/", segments);
    }
}
