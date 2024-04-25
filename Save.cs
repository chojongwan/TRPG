using System;
using System.IO;
using Newtonsoft.Json;

namespace TRPGTest
{

    internal class Save
    {
        string _fileName = "GameDataPlayer.json";

        public void SaveGameData(Player player)
        {
            // 게임 데이터를 JSON 형식으로 직렬화하여 파일에 저장합니다.
            string json = JsonConvert.SerializeObject(player);
            File.WriteAllText(_fileName, json);
            Console.WriteLine("게임 데이터를 저장했습니다.");
        }

        public Player LoadGameData()
        {
            // 파일에서 JSON 형식의 게임 데이터를 읽어와 Player 객체로 역직렬화합니다.
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);
                Player player = JsonConvert.DeserializeObject<Player>(json);
                Console.WriteLine("게임 데이터를 불러왔습니다.");
                return player;
            }
            else
            {
                Console.WriteLine("저장된 게임 데이터가 없습니다.");
                return null;
            }
        }
    }
}
