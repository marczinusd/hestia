using System;

namespace Hestia.Model
{
    public class Repository
    {
       public string Name { get; private set; }
       
       public Directory RootDirectory { get; private set; }
    }
}
