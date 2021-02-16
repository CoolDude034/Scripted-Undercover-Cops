using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;

namespace UndercoverCopsWantedLevel
{
    public class UndercoverCops : Script
    {
        ScriptSettings IniSettings;
        int interval = 25000; // Spawn Interval
        int spawnrange; // Range where enemies will spawn from
        bool DontSpawnOnfoot; // to make it work with assault teams mod
        public UndercoverCops()
        {
            IniSettings = ScriptSettings.Load("scripts\\Config.ini");
            spawnrange = IniSettings.GetValue<int>("CONFIG", "SpawnRange", 100);
            DontSpawnOnfoot = IniSettings.GetValue<bool>("CONFIG", "DontSpawnOnfoot", false);
            Tick += update;
            Interval = interval;
        }

        private void update(object sender, EventArgs e)
        {
            if (DontSpawnOnfoot == true)
            {
                if (Game.Player.Character.IsOnFoot == false)
                {
                    if (Game.Player.WantedLevel >= 1)
                    {
                        CreatePoliceVehicle();
                    }
                }
            }
            else if (DontSpawnOnfoot == false)
            {
                if (Game.Player.WantedLevel >= 1)
                {
                    CreatePoliceVehicle();
                }
            }
        }
        // setting up the undercover police :D
        void CreatePoliceVehicle()
        {
            Vehicle policeCar = World.CreateVehicle(VehicleHash.Police4, Game.Player.Character.Position + (Game.Player.Character.ForwardVector * spawnrange));
            Ped cop1 = World.CreatePed(PedHash.Business01AMY, policeCar.Position);
            cop1.SetIntoVehicle(policeCar, VehicleSeat.Driver);
            policeCar.PlaceOnGround();
            policeCar.PlaceOnNextStreet();
            policeCar.MarkAsNoLongerNeeded();
            cop1.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
            Function.Call(Hash.SET_PED_AS_COP, cop1, true); // set the ped as a cop
            cop1.Task.FightAgainst(Game.Player.Character);
            cop1.MarkAsNoLongerNeeded();
        }
    }
}
