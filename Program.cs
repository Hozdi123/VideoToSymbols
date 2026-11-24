using System.Numerics.Colors;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using OpenCvSharp;


namespace VideoToAscii
{
        class Program()
    {
        // Создаем список символов от темного к белому
        // Create a package of symbols. From black to white
        // "@#%*+!:;.,   "
        private static readonly string asciiChars = "@#%*+!:;.,   ";

        static void Main(string[] args)
        {
        // Берем файл из пути
        // Take file from Path
        Console.WriteLine("Path to you're video.mp4");
        string videoPath = Console.ReadLine();
        string asciiVideo = @videoPath;
        if (asciiVideo == "" && videoPath == "")
            {
                Console.WriteLine("Can't find a file!");
                return;
            }

                // Захват кадра видео
        // Take a frame from video
        var capture = new VideoCapture(asciiVideo);

        // Скорость видео, интерактивная вещь. У всех индивидуальная (У меня моник 165)
        // Speed video, interacrive. All have individe (My monitor 165)
        Console.WriteLine("Your gc. Like: 60, 144, 240. Video fps: " + capture.Fps);
        string fpsstr = Console.ReadLine();
        if (fpsstr == "")
            {
                Console.WriteLine("Wrong gc");
                return;
            }


        // Проверка файла
        // Check a file
        if(!capture.IsOpened())
        {
            Console.WriteLine("Can't find a file!");
            return;
        }



        double aspectRatio = (Double)capture.FrameHeight / capture.FrameWidth;
        // Ширина
        // Width
        int outputWidth = 240;
        // Высота
        // Heigth
        int outputHeigth = (int)(outputWidth * aspectRatio * 0.5);
        if  (outputHeigth > 67)
            {
                outputHeigth = 67;
            }
        Console.SetWindowSize(outputWidth, outputHeigth);


        using var frame = new Mat();
        using var resizedFrame = new Mat();
        using var grayFrame = new Mat();

        // Цикл по кадрам
        // Cycle all Frame
        while (true)
        {
            // Читаем кадр если пустой видео закончилось
            // Read frame if empty video stoped
            if (!capture.Read(frame) || frame.Empty())
            {
                break;
            }


            // Изменяем размер кадра
            // Change size Frame
            Cv2.Resize(frame, resizedFrame, new Size(outputWidth, outputHeigth), interpolation: InterpolationFlags.Cubic);

            // Меняем цвет
            // Change Color
            Cv2.CvtColor(resizedFrame, grayFrame, ColorConversionCodes.BGR2GRAY);

            // Преобразуем кадр в символы
            // Convert frame to symbols
            string asciiFrame = ConvertToAscii(grayFrame);

            // Выводим в консоль
            // Output to consol
            Console.Write(asciiFrame);
            // Скорость воспроизведения (фпс)
            // Speed output fps
            int fps = Convert.ToInt32(fpsstr);
            System.Threading.Thread.Sleep(1000 / fps);
        }
        }
            static string ConvertToAscii(Mat grayFrame)
        {
            var ascii = new StringBuilder();
            int width = grayFrame.Width;
            int heigth = grayFrame.Height;

            // Проходимся по каждому символу для преобразования
            // Go over each character for the transformation 
            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Получаем значения яркости 255(белый) - " " . 0(Черный) - @
                    // Take contrast 255(White) - " ". 0(black) - @
                    byte pixelValue = grayFrame.At<byte>(y, x);

                    // Сопостовляем яркость и подставляем
                    // Check consrast and set
                    int index = (pixelValue * (asciiChars.Length - 1)) / 255;

                    ascii.Append(asciiChars[asciiChars.Length - 1 - index]);
                    // Подставляем символ
                    // Set a symbol
                }
                // Переход на новую строку
                // Move to a new line
                ascii.AppendLine();
            }
            return ascii.ToString(); 
        }
    }
}