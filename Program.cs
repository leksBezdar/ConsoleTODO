using System.Diagnostics;

class Program
{
    static FakeDB db = new();
    static ConsoleMenu consoleMenu = new();

    static void Main()
    {
        while (true)
        {
            var commandNumber = consoleMenu.Draw();
            HandleCommand(commandNumber);
        }
    }

    static void HandleCommand(string commandNumber)
    {
        switch (commandNumber)
        {
            case "1":
                AddUser();
                break;
            case "2":
                DisplayUsers();
                break;
            case "3":
                AddTask();
                break;
            case "4":
                DisplayTasks();
                break;
            default:
                Console.WriteLine("Пожалуйста, выберите корректное значение!\n");
                break;
        }
    }

    static void AddTask()
    {
        Console.Write("Введите заголовок задачи [Не больше 32 символов]: ");
        string title = Console.ReadLine();

        Console.Write("Введите описание задачи [Опционально, не больше 255 символов]: \n");
        string description = Console.ReadLine();

        int oid = db.userTasks.Count() + 1;

        var newTask = new UserTask(oid, title, description);
        db.userTasks.Add(newTask);
    }

    static void DisplayTasks()
    {
        if (db.userTasks.Count() == 0)
        {
            Console.WriteLine("На данный момент не существует ни одной задачи!");
        }
        else
        {
            Console.WriteLine("\nСписок задач: \n");
            foreach (var task in db.userTasks)
            {
                Console.WriteLine(task);
            }
        }
        Console.WriteLine();
    }

    static void AddUser()
    {
        Console.Write("Введите ваш username: ");
        string username = Console.ReadLine();

        int oid = db.users.Count() + 1;

        var newUser = new User(oid, username);
        db.users.Add(newUser);
    }

    static void DisplayUsers()
    {
        if (db.users.Count() == 0)
        {
            Console.WriteLine("На данный момент не существует ни одного пользователя!");
        }
        else
        {
            Console.WriteLine("\nСписок пользователей: \n");
            foreach (var user in db.users)
            {
                Console.WriteLine(user);
            }
        }
        Console.WriteLine();
    }
}

class ConsoleMenu
{
    public string Draw()
    {
        Console.WriteLine("Приветствую! Ниже представлен список доступных команд: \n");
        Console.WriteLine("1. Создать пользователя");
        Console.WriteLine("2. Показать всех пользователей");
        Console.WriteLine("3. Создать задачу");
        Console.WriteLine("4: Показать все задачи");
        Console.Write("\nВыбери команду, указав ее цифру: ");

        while (true)
        {
            string? commandNumber = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(commandNumber))
            {
                return commandNumber;
            }
            Console.Write("\nПожалуйста, выберите корректное значение: ");
        }
    }
}

class UserTask
{
    public int oid;
    public string title;
    public string? description;
    public bool isDone = false;

    public UserTask(int oid, string title, string? description)
    {
        this.oid = oid;
        this.title = title;
        this.description = description;
    }

    public void Done() => this.isDone = true;

    public void Undone() => this.isDone = false;

    public override string ToString()
    {
        string taskDetails = $"ID: {oid}\nTitle: {title} \nDescription: {description ?? "Нет описания"} \nDone: {isDone}";
        string separator = new string('=', taskDetails.Length);

        return $"{separator}\n{taskDetails}\n{separator}\n";
    }

}

class User
{
    public int oid;
    public string username;
    public List<UserTask> tasks = new();
    public bool isSuperuser = false;

    public User(int oid, string username)
    {
        this.oid = oid;
        this.username = username;
    }

    public void AddTask(UserTask task)
    {
        if (task != null)
        {
            this.tasks.Add(task);
        }
    }

    public void RemoveTask(int taskOid)
    {
        var taskToRemove = this.tasks.FirstOrDefault(task => task.oid == taskOid);
        if (taskToRemove != null)
        {
            this.tasks.Remove(taskToRemove);
        }
    }

    public override string ToString() {
        string userDetails = $"id: {this.oid}\nusername: {this.username}";
        string separator = new string('=', userDetails.Length);
        return $"{separator}\n{userDetails}\n{separator}\n";
    }
}

class FakeDB
{
    public List<User> users = new();
    public List<UserTask> userTasks = new();
}
