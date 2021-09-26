using System;
using System.Collections.ObjectModel;

namespace GProof.Alerta.Models
{
    public static class Methods
    {
        public static ObservableCollection<GovCity> SetHebrewParent(ObservableCollection<GovCity> cityCollection)
        {
            foreach(var city in cityCollection)
            {
                try
                {
                    char[] cArray = city.שם_ישוב.ToCharArray();
                    for (int i = 0; i < city.שם_ישוב.Length; i++)
                    {
                        if (cArray[i] == '(')
                        {
                            cArray[i] = ')';
                        }
                      
                    }
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }
            return cityCollection;
        }
    }
}
