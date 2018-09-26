using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.ResourceManagement;

namespace CMSWeb
{
    public class ResourcesBuilder : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            builder.Add().DefineScript("jQuery").SetUrl("jquery-1.5.2.min.js");
        }
    }
}
