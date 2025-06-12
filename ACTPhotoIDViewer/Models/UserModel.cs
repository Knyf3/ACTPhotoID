using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTPhotoIDViewer.Models
{
    public class UserModel
    {
            public int UserNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int CardNumber { get; set; }
            public byte[]? Photo { get; set; }


        
    }

}
