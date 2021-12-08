using System;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Collections.Generic;


namespace StoreProductsSQL
{
    class Product
    {
        static string connection = "Data Source = .;Initial Catalog = Store; Integrated Security = True";

        public static void Menu()
        {
            string b;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine();
                System.Console.WriteLine($"    | ADD = 1 |     | SEARCH = 2 |   | DELETE = 3 |  | RECOVERY = 4 |  | SHOW = 5 | ");
                System.Console.WriteLine();
                Console.ResetColor();
                System.Console.Write("                                         Enter : ");
                string a = Console.ReadLine().ToLower();

                switch (a)
                {
                    case "add":case "1":
                        Add();
                        break;
                    case "search":case "2":
                       Search();
                        break;
                    case "delete":case "3":
                       Delete();
                        break;
                    case "recovery":case "4":
                       Recovery();
                        break;
                    case "show":case "5":
                       SHOW();
                        break;            
                    default:
                    System.Console.WriteLine();
                    System.Console.WriteLine("********* In Darkhasti Mojood nist **********");
                    System.Console.WriteLine();
                        break;
                }
                System.Console.WriteLine();
                System.Console.Write("darkhast digari darid ( y - n )  : ");
                b = Console.ReadLine().ToLower();

            } while (b == "y");
        }
        public static void Add()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine();
            System.Console.WriteLine("----------------------------------------------- ADD -------------------------------------------------");
            System.Console.WriteLine();
            Console.ResetColor();
            VMProduct sample = new VMProduct();
            System.Console.Write("Enter Name Product : ");
            sample.Name_Product = Console.ReadLine();

            System.Console.Write("Enter Category Product : ");
            sample.Category = Console.ReadLine();

            System.Console.Write("Enter Price Product : ");
            sample.Price = int.Parse(Console.ReadLine());

            System.Console.Write("Chand Darsad Takhfif : ");
            sample.Discount = int.Parse(Console.ReadLine());

            System.Console.Write("Upload Original Product photo : ");
            sample.Original_photo = Console.ReadLine();

            System.Console.Write("Tozihati dar Morede Mahsool : ");
            sample.Description = Console.ReadLine();

            sample.Flag=1;


            using (SqlConnection Db = new SqlConnection(connection))
            {
                string b = "Insert Into Product values(@name,@category,@price,@darsad,@Ophoto,@tozihat,@flag)";
                Db.Execute(b, new { name = sample.Name_Product, category = sample.Category, price = sample.Price, darsad = sample.Discount, Ophoto = sample.Original_photo, tozihat = sample.Description ,flag=sample.Flag});
            }
            string B;
            do
            {
                System.Console.Write("Enter Brand: ");
                sample.Name_Brand = Console.ReadLine();
                using (SqlConnection Db = new SqlConnection(connection))
                {
                    int id1 = Db.Query<int>("SELECT MAX(Id_Product) FROM Product").SingleOrDefault();
                    string b = "Insert Into Brand values(@id,@name)";
                    Db.Execute(b, new { id = id1, name = sample.Name_Brand });
                    // string a = "select * From Product inner join Brand on Product.Id_Product = Brand.Id_brand inner join color on Product.Id_Product = Color.Id_color inner join Photo on Product.Id_Product = Photo.Id_photo inner join Size on Product.Id_Product = Size.Id_size";
                    // Db.Execute(a);
                }

                System.Console.Write("Baz ham Brand Ezafe mikonid (y-n)? : ");
                B = Console.ReadLine().ToLower();
            } while (B == "y");

            string S;
            do
            {
                System.Console.Write("Enter size : ");
                sample.size = Console.ReadLine();

                using (SqlConnection Db = new SqlConnection(connection))
                {
                    int id1 = Db.Query<int>("SELECT MAX(Id_Product) FROM Product").SingleOrDefault();
                    string b = "Insert Into Size values(@id,@size)";
                    Db.Execute(b, new { id = id1, size = sample.size });
                }

                System.Console.Write("Baz ham Size Ezafe mikonid (y-n)? : ");
                S = Console.ReadLine().ToLower();
            } while (S == "y");

            string C;
            do
            {
                System.Console.Write("Enter Color : ");
                sample.color = Console.ReadLine();

                using (SqlConnection Db = new SqlConnection(connection))
                {
                    int id1 = Db.Query<int>("SELECT MAX(Id_Product) FROM Product").SingleOrDefault();
                    string b = "Insert Into Color values(@id,@color)";
                    Db.Execute(b, new { id = id1, color = sample.color });
                }

                System.Console.Write("Baz ham Color Ezafe mikonid (y-n)? : ");
                C = Console.ReadLine().ToLower();
            } while (C == "y");

            string img;
            do
            {
                System.Console.Write("Upload More Photo : ");
                sample.More_photo = Console.ReadLine();

                using (SqlConnection Db = new SqlConnection(connection))
                {
                    int id1 = Db.Query<int>("SELECT MAX(Id_Product) FROM Product").SingleOrDefault();
                    string b = "Insert Into Photo values(@id,@photo)";
                    Db.Execute(b, new { id = id1, photo = sample.More_photo });
                }

                System.Console.Write("Aya baz ham photo Upload mikonid (y-n)? : ");
                img = Console.ReadLine().ToLower();
            } while (img == "y");

            System.Console.WriteLine("--------------------- Ba Movafaghiat ADD Shod -------------------------");
        }

        public static void Search()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine();
            System.Console.WriteLine("----------------------------------------------- SEARCH -------------------------------------------------");
            System.Console.WriteLine();
            Console.ResetColor();
            System.Console.Write("ENTER : ");
            string search = Console.ReadLine();
            System.Console.WriteLine();

            using (SqlConnection Db = new SqlConnection(connection))
            {
                var a = Db.Query<VMProduct>($"select * From Product inner join Brand on Product.Id_Product = Brand.Id_brand inner join Color on  Product.Id_Product = Color.Id_color Where Name_product Like '%{search}%'").ToList();

                System.Console.WriteLine("------------------------------- Result --------------------------------");
                System.Console.WriteLine();
                foreach (var item in a)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine($"Product : Name: {item.Name_Product} ----- Brand: {item.Name_Brand} ----- Price: {item.Price} ----- Color: {item.color} ---- {item.Discount}% Takhfif");
                    System.Console.WriteLine();
                }
            }

        }

        public static void Delete()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine();
            System.Console.WriteLine("----------------------------------------------- DELETE -------------------------------------------------");
            System.Console.WriteLine();
            Console.ResetColor();
            System.Console.Write("Enter Id Product for Delete : ");
            int id = int.Parse(Console.ReadLine());

            using (SqlConnection Db = new SqlConnection(connection))
            {
                 string a ="update Product set Flag=0 where Id_Product=@x";
                 Db.Execute(a,new{x=id});
            }
            System.Console.WriteLine();
            System.Console.WriteLine("-------------- Ba movafaghiat Hazf shod --------------");
        }
        public static void Recovery()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine();
            System.Console.WriteLine("----------------------------------------------- Recovery -------------------------------------------------");
            System.Console.WriteLine();
            Console.ResetColor();
            System.Console.Write("Id Product for Recovery : ");
            int id = int.Parse(Console.ReadLine());
            using (SqlConnection Db = new SqlConnection(connection))
            {
                 string a ="update Product set Flag=1 where Id_Product=@x";
                 Db.Execute(a,new{x=id});
            }
            System.Console.WriteLine();
            System.Console.WriteLine("--------------Mahsool ba movafaghiat Bargasht --------------");
        }

        public static void SHOW()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine();
            System.Console.WriteLine("------------------------------------------- SHOW Product -------------------------------------------");
            System.Console.WriteLine();
            Console.ResetColor();
            using (SqlConnection Db = new SqlConnection(connection))
            {
               var a =Db.Query<VMProduct>($"select * From Product inner join Brand on Product.Id_Product = Brand.Id_brand inner join Color on  Product.Id_Product = Color.Id_color inner join  Photo on Product.Id_Product = Id_photo inner join Size on Product.Id_Product = Id_size Where Flag =1").ToList();
               foreach (var item in a)
               {
                System.Console.WriteLine();
                System.Console.WriteLine($"Product Id {item.Id_Product}");
                System.Console.WriteLine($"Name: {item.Name_Product} ----- Brand: {item.Name_Brand} ----- Price: {item.Price.ToString("#,0")} ----- Color: {item.color} ---- {item.Discount}% Takhfif");
                System.Console.WriteLine($"Product : Size: {item.size} ----- Picture: {item.Original_photo}  ----- Caption: {item.Description}");
                System.Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
                
               }
            }

        }
    }
    public class VMProduct
    {
        public int Id_Product { get; set; }
        public string Name_Product { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string Name_Brand { get; set; }
        // public int Id_Brand { get; set; }
        public string size { get; set; }
        //  public int Id_Size { get; set; }
        public string color { get; set; }
        // public int Id_color { get; set; }
        public string Original_photo { get; set; }
        public string More_photo { get; set; }
        //  public int Id_photo { get; set; }
        public string Description { get; set; }
        public int Flag { get; set; }
        
        




    }
}

