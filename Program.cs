using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Threading.Channels;

public class Program
{
    // 메인 메서드
    public static void Main(string[] args)
    {
        ShowMainMenu();
    }
    static Player player = new Player(); // 플레이어 객체 생성

    //열거형 구문을 사용하는 이유 유지보수 및 가독성을 위해서
    public enum ItemType                // 열거형 (아이템 종류, 종류 구분으로 공격력 방어력을 구분)
    {
        Weapon,
        Armor
    }

    // 아이템 클래스 정의
    class Item          //프로퍼티
    {
        public string Name { get; set; } // 아이템 이름
        public string Effect { get; set; } // 아이템 효과
        public string Description { get; set; } // 아이템 설명
        public int Price { get; set; } // 아이템 가격
        public bool IsPurchased { get; set; } = false; // 아이템 구매 여부
        public ItemType Type { get; set; } // 아이템 종류
        public bool IsEquipped { get; set; } //아이템 장착 여부

        public Item(string name, string effect, string description, int price, ItemType type)
        {
            Name = name;
            Effect = effect;
            Description = description;
            Price = price;
            Type = type;
        }
    }

    // 상점의 아이템 목록
    static Item[] shopItems = new Item[]
    {
        new Item("가죽 갑옷", "방어력 +5", "기본적인 방어력을 가진 갑옷입니다.", 1000, ItemType.Armor),
        new Item("무쇠갑옷", "방어력 +17", "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, ItemType.Armor),
        new Item("스파르타의 갑옷", "방어력 +30", "스파르타의 전사들이 사용했다는 전설등급 갑옷입니다.", 3500, ItemType.Armor),
        new Item("아테네의 갑옷", "방어력 +50", "올림푸스의 신 '아테네'가 사용했다는 신화등급 갑옷입니다.", 7000, ItemType.Armor),
        new Item("낡은 검", "공격력 +2", "쉽게 볼 수 있는 낡은 검입니다.", 600, ItemType.Weapon),
        new Item("청동 도끼", "공격력 +5", "어디선가 사용됐던거 같은 도끼입니다.", 1500, ItemType.Weapon),
        new Item("스파르타의 창", "공격력 +7", "스파르타의 전사들이 사용했다는 전설등급 창입니다.", 3500, ItemType.Weapon),
        new Item("헤라클래스의 몽둥이", "공격력 +15", "올림푸스의 신화적 전사 헤라클레스가 사용했다는 신화등급의 몽둥이입니다.", 7000, ItemType.Weapon),
        new Item("제우스의 아스트라페", "공격력 +50", "올림푸스 그 자체인 제우스가 사용했다는 고유무기 입니다.", 30000, ItemType.Weapon)
    };


    // 플레이어 클래스 정의
    class Player
    {
        public int Gold { get; set; } = 800; // 초기 골드
        public int LV { get; set; } = 1;
        public int Attack { get; set; } = 5;
        public int Defense { get; set; } = 5;
        public int HP { get; set; } = 100;
        public List<Item> Inventory { get; } = new List<Item>(); // 인벤토리 리스트
    }

    // 메인 메뉴 표시
    static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine(@" $$$$$$\                                           $$$$$$\    $$\                          $$\           $$\       $$\ 
$$  __$$\                                         $$  __$$\   $$ |                         $$ |          $$ |      $$ |
$$ /  \__| $$$$$$\  $$$$$$\$$$$\   $$$$$$\        $$ /  \__|$$$$$$\    $$$$$$\   $$$$$$\ $$$$$$\         $$ |      $$ |
$$ |$$$$\  \____$$\ $$  _$$  _$$\ $$  __$$\       \$$$$$$\  \_$$  _|   \____$$\ $$  __$$\\_$$  _|        $$ |      $$ |
$$ |\_$$ | $$$$$$$ |$$ / $$ / $$ |$$$$$$$$ |       \____$$\   $$ |     $$$$$$$ |$$ |  \__| $$ |          \__|      \__|
$$ |  $$ |$$  __$$ |$$ | $$ | $$ |$$   ____|      $$\   $$ |  $$ |$$\ $$  __$$ |$$ |       $$ |$$\                     
\$$$$$$  |\$$$$$$$ |$$ | $$ | $$ |\$$$$$$$\       \$$$$$$  |  \$$$$  |\$$$$$$$ |$$ |       \$$$$  |      $$\       $$\ 
 \______/  \_______|\__| \__| \__| \_______|       \______/    \____/  \_______|\__|        \____/       \__|      \__|
                                                                                                                       
                                                                                                                       
                                                                                                                       ");                                          //시작할때 로고
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
        Console.WriteLine("0. 게임종료");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전");
        //Console.WriteLine("5. 저장하기\n");
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        string input = Console.ReadLine();
        switch (input)
        {
            case "0":
                Environment.Exit(0); // 프로그램 종료
                break;
            case "1":
                ShowStatus();
                break;
            case "2":
                ShowInventory();
                break;
            case "3":
                ShowShop();
                break;
            case "4":
                ShowDungeon();
                break;
            //case "5":
            //    SavePlayerData();
            //    break;
            default:
                Console.WriteLine("잘못된 입력입니다.");
                ShowMainMenu();
                break;
        }
    }
    // 상태 보기 기능 구현
    static void ShowStatus()
    {
        Console.Clear();
        Console.WriteLine($"LV: {player.LV}");
        Console.WriteLine("Chad (전사)");
        Console.WriteLine($"공격력: {player.Attack}");
        Console.WriteLine($"방여력: {player.Defense}");
        Console.WriteLine($"체력: {player.HP}");
        Console.WriteLine($"Gold: {player.Gold} G\n");
        Console.WriteLine("0. 나가기\n");
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        string input = Console.ReadLine();
        if (input == "0")
            ShowMainMenu();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowStatus();
        }
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    // 인벤토리 표시 기능 구현
    static void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

        if (player.Inventory.Count == 0)                    //Inventory은 아이템 리스트
        {
            Console.WriteLine("보유 중인 아이템이 없습니다.");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "0")
                ShowMainMenu();
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowInventory();
            }
        }
        else
        {
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                string equipped = player.Inventory[i].IsEquipped ? "[E]" : "";                  // 아이템이 장착되었는지 확인하여 표시, 삼항 연산자-> equipped
                Console.WriteLine($"{i + 1}. {equipped}{player.Inventory[i].Name}");
            }
            Console.WriteLine();
            Console.WriteLine("장착할 장비의 번호를 입력해주세요:");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            if (input == "0")
                ShowMainMenu();
            else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= player.Inventory.Count)        //selectedIndex 인벤토리에서 해당 아이템을 찾을때 사용 (out 인자를 사용)
            {
                EquipItem(selectedIndex - 1); // 선택한 아이템을 장착합니다.    -1을 하는 이유 받은 값은 아이템 번호 1이다 1을 보내야하는데 받는곳은 배열을 사용하니 -1을해서 배열 정렬을 하는것
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowInventory();
            }
        }
    }

    // 아이템 장착 기능 구현
    static void EquipItem(int selectedIndex)
    {
        //현재 플레이거 가지고 있는 아이템을 selected로 가져오는 것
        Item selected = player.Inventory[selectedIndex];

        if (selected.IsEquipped)
        {
            Console.WriteLine("이미 장착된 아이템입니다.");
        }
        else
        {
            // 이미 장착된 아이템의 수를 세어봅니다.
            int equippedItemCount = 0;
            foreach (var item in player.Inventory)
            {
                if (item.IsEquipped)
                    equippedItemCount++;
            }

            // 이미 2개의 아이템을 장착했는지 확인합니다.
            if (equippedItemCount >= 2)
            {
                Console.WriteLine("더 이상 아이템을 장착할 수 없습니다.");
            }
            else
            {
                // 선택한 아이템을 장착합니다.
                selected.IsEquipped = true;

                // 선택한 아이템의 효과를 능력치에 반영합니다.
                switch (selected.Type)
                {
                    case ItemType.Weapon:
                        player.Attack += GetEffectValue(selected.Effect);
                        break;
                    case ItemType.Armor:
                        player.Defense += GetEffectValue(selected.Effect);
                        break;
                }
                Console.WriteLine($"{selected.Name}을(를) 장착했습니다.");
            }
        }
        // 다시 원하는 행동을 입력받습니다.
        Console.WriteLine("새로고침 입력 0.");
        Console.WriteLine("장착 해재 할 아이템 선택 1.");
        string input = Console.ReadLine();

        if (input == "0")
        {
            // 다시 인벤토리를 표시합니다.
            ShowInventory();
        }
        else if (input == "1")
        {
            ShowUnequipMenu();
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            // 다시 인벤토리를 표시합니다.
            ShowInventory();
        }
    }
    // 아이템 해제 기능 구현
    static void UnequipItem(int selectedIndex)
    {
        // 선택한 아이템을 가져옵니다.
        Item selected = player.Inventory[selectedIndex];

        // 선택한 아이템이 이미 해제된 상태인지 확인합니다.
        if (!selected.IsEquipped)
        {
            Console.WriteLine("선택한 아이템은 이미 해제되었습니다.");
            return;
        }

        // 선택한 아이템을 해제합니다.
        selected.IsEquipped = false;

        // 선택한 아이템의 효과를 능력치에서 제거합니다.
        switch (selected.Type)
        {
            case ItemType.Weapon:
                player.Attack -= GetEffectValue(selected.Effect);
                break;
            case ItemType.Armor:
                player.Defense -= GetEffectValue(selected.Effect);
                break;
        }

        Console.WriteLine($"{selected.Name}을(를) 해제했습니다.");
    }

    // 아이템 해제를 위한 인벤토리 표시 기능 구현
    static void ShowUnequipMenu()
    {
        Console.Clear();
        Console.WriteLine("장착 중인 아이템 목록");
        Console.WriteLine("장착 중인 아이템을 해제할 수 있습니다.\n");

        // 장착 중인 아이템 목록을 표시합니다.
        bool hasEquippedItems = false;
        for (int i = 0; i < player.Inventory.Count; i++)
        {
            if (player.Inventory[i].IsEquipped)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
                hasEquippedItems = true;
            }
        }

        if (!hasEquippedItems)
        {
            Console.WriteLine("장착 중인 아이템이 없습니다.");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            if (input == "0")
                ShowMainMenu();
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowUnequipMenu();
            }
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("해제할 아이템의 번호를 입력해주세요:");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            string input = Console.ReadLine();
            if (input == "0")
                ShowMainMenu();
            else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= player.Inventory.Count)
            {
                UnequipItem(selectedIndex - 1); // 선택한 아이템을 해제합니다.
                ShowInventory();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                ShowUnequipMenu();
            }
        }
    }

    // 아이템 효과 값을 가져오는 메서드
    static int GetEffectValue(string effect)
    {
        // 아이템 효과 문자열에서 숫자 부분을 추출하여 반환합니다.
        // 예: "방어력 +5"에서 5를 추출하여 반환합니다.
        int value = 0;
        foreach (char c in effect)
        {
            if (char.IsDigit(c))
            {
                value = value * 10 + (c - '0');
            }
        }
        return value;
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    // 상점 표시 기능 구현
    static void ShowShop()
    {
        // 화면을 깨끗하게 지우고 상점 정보를 출력합니다.
        Console.Clear();
        Console.WriteLine("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
        Console.WriteLine($"[보유 골드] {player.Gold} G\n");
        Console.WriteLine("[아이템 목록]");

        // 상점에 있는 모든 아이템을 순회하며 출력합니다.
        for (int i = 0; i < shopItems.Length; i++)
        {
            // 아이템의 구매 상태를 표시합니다.
            string purchaseStatus = shopItems[i].IsPurchased ? "구매완료" : $"{shopItems[i].Price} G";
            Console.WriteLine($"{i + 1}. {shopItems[i].Name} | {shopItems[i].Effect} | {shopItems[i].Description} | {purchaseStatus}");
        }

        Console.WriteLine();
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine("0. 나가기\n");
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        // 사용자 입력을 받아서 해당 기능을 실행합니다.
        string input = Console.ReadLine();
        if (input == "1")
            BuyItem();
        else if (input == "0")
            ShowMainMenu();
        else if (input == "2")
            SellItem();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowShop(); // 재귀 호출을 통해 다시 상점 메뉴를 표시합니다.
        }
    }

    // 아이템 구매 기능 구현
    static void BuyItem()
    {
        Console.WriteLine("구매할 아이템의 번호를 입력해주세요.");
        string input = Console.ReadLine();

        // 입력이 유효한지 확인합니다.
        if (int.TryParse(input, out int itemIndex) && itemIndex >= 1 && itemIndex <= shopItems.Length)
        {
            itemIndex--; // 배열 인덱스로 변환, 배열의 인덱스는 0부터 시작하기 때문에 번호를 1번부터 하기위해

            // 이미 구매한 아이템인지 확인합니다.
            if (shopItems[itemIndex].IsPurchased)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
            }
            else if (player.Gold >= shopItems[itemIndex].Price)
            {
                // 골드가 충분한지 확인하고 구매합니다.
                player.Gold -= shopItems[itemIndex].Price; // 골드 차감
                player.Inventory.Add(shopItems[itemIndex]); // 인벤토리에 아이템 추가
                shopItems[itemIndex].IsPurchased = true; // 구매 완료 표시
                Console.WriteLine("구매를 완료했습니다.");
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
        }
        // 다시 원하는 행동을 입력받습니다.
        Console.WriteLine("\n원하시는 행동을 입력해주세요.");
        input = Console.ReadLine();

        if (input == "0")
        {
            ShowMainMenu();
        }
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowShop(); // 재귀 호출을 통해 다시 상점 메뉴를 표시합니다.
        }
    }
    // 판매 후에 구매 상태를 초기화하여 다시 구매할 수 있도록 만듭니다.
    static void SellItem()
    {
        Console.Clear();
        Console.WriteLine("판매할 아이템을 선택해주세요:\n");

        // 인벤토리에 있는 모든 아이템을 표시합니다.
        for (int i = 0; i < player.Inventory.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {player.Inventory[i].Name} - {player.Inventory[i].Price / 2} G");
        }

        Console.WriteLine("\n0. 나가기");

        string input = Console.ReadLine();

        if (input == "0")
        {
            ShowShop();
        }
        else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= player.Inventory.Count)
        {
            selectedIndex--; // 배열 인덱스로 변환
            Item selectedItem = player.Inventory[selectedIndex];

            // 아이템을 판매하여 골드를 획득합니다.
            int sellingPrice = selectedItem.Price / 2; // 판매 가격은 원가의 절반입니다.
            player.Gold += sellingPrice; // 골드 증가
            player.Inventory.RemoveAt(selectedIndex); // 인벤토리에서 아이템 제거

            // 판매 후에 구매 상태를 초기화합니다.
            for (int i = 0; i < shopItems.Length; i++)
            {
                if (shopItems[i].Name == selectedItem.Name)
                {
                    shopItems[i].IsPurchased = false; // 구매 상태 초기화
                    break;
                }
            }

            Console.WriteLine($"\n{selectedItem.Name}을(를) 판매했습니다. (+{sellingPrice} G)\n");
        }
        else
        {
            Console.WriteLine("\n잘못된 입력입니다.\n");
        }

        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey();
        ShowShop();
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    //던전 구성
    static void ShowDungeon()
    {
        Console.Clear();
        if (player.HP < 0)      //HP가 0이될때 게임 오버 구문
        {
            GameOver();
        }
            Console.WriteLine("던전");
        Console.WriteLine("전투로 골드를 얻을 수 있는 던전입니다.\n");
        Console.WriteLine("요구능력치 : 방어력, 공격력 : 전리품 증가");
        Console.WriteLine($"[현재 HP] {player.HP} \n");
        Console.WriteLine($"[현재 Gold] {player.Gold}G \n");
        Console.WriteLine("[던전 목록]");

        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 쉬움 [요구 능력치 15]");
        Console.WriteLine("2. 보통 [요구 능력치 30]");
        Console.WriteLine("3. 어려움 [요구 능력치 60]");

        string input = Console.ReadLine();
        if (input == "0")
            ShowMainMenu();
        else if (input == "1")
            Dungeon1();
        else if (input == "2")
            Dungeon2();
        else if (input == "3")
            Dungeon3();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowDungeon(); // 재귀 호출을 통해 다시 던전 메뉴를 표시합니다.
        }
    }
    //쉬움
    static void Dungeon1()
    {
        bool clear = ClearDungeon(15, player.Defense); // 쉬운 던전의 권장 방어력은 15입니다.
        if (clear)
        {
            // 클리어 보상 계산
            int baseReward = 1000; // 쉬운 던전의 기본 클리어 보상
            int reward = Compensation(baseReward, player.Attack);

            // 보상 획득 등의 처리
            player.Gold += reward;
            Console.WriteLine($"던전을 클리어했습니다! 보상으로 {reward} G를 획득하였습니다.");
        }
        else
        {
            // 실패 시의 처리
            Random rand = new Random();
            int hpLoss = rand.Next(20, 36) / 2; // 기본 체력 감소량은 20 ~ 35입니다.
            player.HP -= hpLoss;
            Console.WriteLine($"던전 클리어에 실패했습니다. 체력이 {hpLoss}만큼 감소하였습니다.");
        }
        Console.WriteLine("0.던전으로 돌아가기");
        string input = Console.ReadLine();
        if (input == "0")
            ShowDungeon();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowDungeon(); // 재귀 호출을 통해 다시 던전 메뉴를 표시합니다.
        }
    }
    //보통
    static void Dungeon2()
    {
        bool clear = ClearDungeon(30, player.Defense); // 보통 던전의 권장 방어력은 30입니다.
        if (clear)
        {
            // 클리어 보상 계산
            int baseReward = 2000; // 보통 던전의 기본 클리어 보상
            int reward = Compensation(baseReward, player.Attack);

            // 보상 획득 등의 처리
            player.Gold += reward;
            Console.WriteLine($"던전을 클리어했습니다! 보상으로 {reward} G를 획득하였습니다.");
        }
        else
        {
            // 실패 시의 처리
            Random rand = new Random();
            int hpLoss = rand.Next(20, 36) / 2; // 기본 체력 감소량은 20 ~ 35입니다.
            player.HP -= hpLoss;
            Console.WriteLine($"던전 클리어에 실패했습니다. 체력이 {hpLoss}만큼 감소하였습니다.");
        }
        Console.WriteLine("0.던전으로 돌아가기");
        string input = Console.ReadLine();
        if (input == "0")
            ShowDungeon();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowDungeon(); // 재귀 호출을 통해 다시 던전 메뉴를 표시합니다.
        }
    }
    //하드
    static void Dungeon3()
    {
        bool clear = ClearDungeon(60, player.Defense); // 어려움 던전의 권장 방어력은 60.
        if (clear)
        {
            // 클리어 보상 계산
            int baseReward = 5000; // 어려움 던전의 기본 클리어 보상
            int reward = Compensation(baseReward, player.Attack);

            // 보상 획득 등의 처리
            player.Gold += reward;
            Console.WriteLine($"던전을 클리어했습니다! 보상으로 {reward} G를 획득하였습니다.");
        }
        else
        {
            // 실패 시의 처리
            Random rand = new Random();
            int hpLoss = rand.Next(40, 56) / 2; // 기본 체력 감소량은 40 ~ 55입니다.
            player.HP -= hpLoss;
            Console.WriteLine($"던전 클리어에 실패했습니다. 체력이 {hpLoss}만큼 감소하였습니다.");
        }
        Console.WriteLine("0.던전으로 돌아가기");
        string input = Console.ReadLine();
        if (input == "0")
            ShowDungeon();
        else
        {
            Console.WriteLine("잘못된 입력입니다.");
            ShowDungeon(); // 재귀 호출을 통해 다시 던전 메뉴를 표시합니다.
        }
    }
    //클리어시 얻는 전리품 계산
    static int Compensation(int baseReward, int attack)
    {
        Random rand = new Random();
        int attackreward = rand.Next(attack, attack * 2 + 1); // 공격력 * 1 ~ 2의 범위에서 %
        return baseReward + attackreward;
    }
    //클리어, 실패 판별
    static bool ClearDungeon(int capability, int defense)
    {
        if (defense < capability)
        {
            Random rand = new Random();
            return rand.Next(1, 101) > 40; // 40%의 확률로 실패
        }
        return true; // 클리어 가능
    }
    //게임 오버기능
    public static void GameOver()
    {
        player.HP = 0;
        Console.WriteLine(@"  /$$$$$$                                           /$$$$$$                                      /$$       /$$
 /$$__  $$                                         /$$__  $$                                    | $$      | $$
| $$  \__/  /$$$$$$  /$$$$$$/$$$$   /$$$$$$       | $$  \ $$ /$$    /$$ /$$$$$$   /$$$$$$       | $$      | $$
| $$ /$$$$ |____  $$| $$_  $$_  $$ /$$__  $$      | $$  | $$|  $$  /$$//$$__  $$ /$$__  $$      | $$      | $$
| $$|_  $$  /$$$$$$$| $$ \ $$ \ $$| $$$$$$$$      | $$  | $$ \  $$/$$/| $$$$$$$$| $$  \__/      |__/      |__/
| $$  \ $$ /$$__  $$| $$ | $$ | $$| $$_____/      | $$  | $$  \  $$$/ | $$_____/| $$                          
|  $$$$$$/|  $$$$$$$| $$ | $$ | $$|  $$$$$$$      |  $$$$$$/   \  $/  |  $$$$$$$| $$             /$$       /$$
 \______/  \_______/|__/ |__/ |__/ \_______/       \______/     \_/    \_______/|__/            |__/      |__/
                                                                                                              
                                                                                                              
                                                                                                              ");
        Console.WriteLine("게임 오버!!! 체력이 0이 되었습니다");
        Console.WriteLine();
        Console.WriteLine("다시 시작 0 입력");
        string re = Console.ReadLine();
        if (re == "0")
        {
            player.HP = 100;
            ShowMainMenu();
        }
        else
        {
            player.HP = 100;
            ShowMainMenu();

        }
    }
}