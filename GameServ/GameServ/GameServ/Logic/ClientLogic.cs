using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServ.Utils;
namespace GameServ.Logic
{
    class ClientLogic
    {
        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="bufferbyte"></param>
        public void Login (Bufferbyte bufferbyte) {
            try
            {
                int count = bufferbyte.ReadInt();
                string password = bufferbyte.ReadString();
                Console.WriteLine("Login count =" + count + ",password=" + password);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + e);
            }
            finally {
                bufferbyte.Clear();
            }
        }
        /// <summary>
        /// Register Method
        /// </summary>
        /// <param name="bufferbyte"></param>
        public void Register(Bufferbyte bufferbyte) {

        }
    }
}
