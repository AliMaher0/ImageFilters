using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageFilters
{
    public class ImageOperations
    {
        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            byte[,] Buffer = new byte[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


        /////----------------------------------------------------Counting Sort-------------------------------------------------------------------------------------
       

        public static List<byte> COUNTING_SORT(byte[] Array)
        {
            int n = Array.Length; //O(1)
            int max = 0; //O(1)
            //find largest element in the Array
            for (int i = 0; i < n; i++)  //O(N)
            {
                if (max < Array[i])  //O(1)
                {
                    max = Array[i];  //O(1)
                } 
            } //O(N)

            //Create a freq array to store number of occurrences of 
            //each unique elements in the given array 
            int[] freq = new int[max + 1];  //O(1)
            for (int i = 0; i < max + 1; i++)  //O(N)
            {
                freq[i] = 0;   //O(1)
            }  //O(N)
            for (int i = 0; i < n; i++)  //O(N)
            {
                freq[Array[i]]++;  //O(1)
            }    //O(N)

            //sort the given array using freq array
            for (int i = 0, j = 0; i <= max; i++)   //O(max)
            {
                while (freq[i] > 0) // //O(max)
                {
                    Array[j] = (byte)i;  //O(1)
                    j++;   //O(1)
                    freq[i]--;   //O(1)
                }
            }//O(n^2)
            List<byte> list1 = new List<byte>(Array);  //O(N)
            return list1;  //O(1)
        }//O(N^2)


        /////////////-------------------------------Quick Sort---------------------------------------------------------------------------------------------

        static void swap(ref byte x, ref byte y)
        {

            byte tempswap = x;//O(1)
            x = y;//O(1)
            y = tempswap;//O(1)
        } //O(1)

        public static int partition(byte[] arr, int low, int high)
        {
            byte pivot = arr[high]; //O(1)   // pivot 
            int i = (low - 1); //O(1)  // Index of smaller element

            for (int j = low; j <= high - 1; j++)//O(high-low-1)
            {
                // If current element is smaller than or
                // equal to pivot
                if (arr[j] <= pivot) //O(1)
                {
                    i++;  //O(1)  // increment index of smaller element
                    swap(ref arr[i], ref arr[j]);//O(1)
                }
            }//O(1)*O(N)	“N= high-low-1”
            swap(ref arr[i + 1], ref arr[high]);//O(1)
            return (i + 1);//O(1)

        }//O(N)

        private static List<byte> Quick_Sort(byte[] arr, int low, int high)
        {
            if (low < high)//O(1)
            {
                /* pi is partitioning index, arr[p] is now
                   at right place */
                int pi = partition(arr, low, high);//O(N)

                // Separately sort elements before
                // partition and after partition
                Quick_Sort(arr, low, pi - 1);//O(N-1)
                Quick_Sort(arr, pi + 1, high);//Not entered here
            }
            List<byte> list1 = new List<byte>(arr);//O(N)
            return list1;//O(1)

        }//O(N^3)

        ///-----------------------------------------Kth Element----------(New)------------------------

        //Minimum heap sort
        public static byte Kthsmalest(byte[] arr, int k)
        {
            int n = arr.Length;//O(1)
            byte[] heap = new byte[k];//O(1)
            for (int i = 0; i < k; i++)//O(k)-->N
            {
                heap[i] = arr[i]; //O(1)
            }

            // build heap
            for (int p = (k - 1) / 2; p >= 0; p--)//O(P)   P<K -->N
            {
                MaxHeapify(heap, k, p);//O(NLogN)
            }
            for (int i = k; i < n; i++)//O(n-k-1)
            {
                if (arr[i] < heap[0])//O(1)
                {
                    heap[0] = arr[i];//O(1)
                    MaxHeapify(heap, k, 0);//O(NLogN)
                }
            }

            return heap[0];//O(1)
        }//O(N^2((LogN))

        public static void MaxHeapify(byte[] heap, int heapSize, int index)
        {
            int l = index * 2 + 1;//O(1)
            int r = index * 2 + 2;//O(1)

            int largest = (l < heapSize && heap[index] < heap[l]) ? l : index;//O(1)
            if (r < heapSize && heap[r] > heap[largest])//O(1)
                largest = r;//O(1)

            if (largest != index)//O(1)
            {
                Exch(heap, index, largest);//O(1)
                MaxHeapify(heap, heapSize, largest);//O(NLogN)
            }
        }//O(NlogN)

        private static void Exch(byte[] arr, int l, int r)
        {
            try
            {
                byte temp = arr[l];//O(1)
                arr[l] = arr[r];//O(1)
                arr[r] = temp;//O(1)
            }
            catch
            {
                Console.Write(l + " ");//O(1)
                Console.WriteLine(r);//O(1)
            }
        }//O(1)

        public static List<byte> check(byte[,] ImageMatrix, int Ws, int i, int j)
        {
            //start from current i and current j to i+2 & j+2 
            //loop on each pixel in current window
            List<byte> current_window = new List<byte>();//O(1)

            for (int Wind_i = 0; Wind_i < Ws; Wind_i++)//O(Ws)
            {
                for (int Wind_j = 0; Wind_j < Ws; Wind_j++)//O(Ws)
                    current_window.Add(ImageMatrix[Wind_i + i, Wind_j + j]);//O(1)

            }
            return current_window;//O(1)

        }//O(Ws^2)


        public static void adaptive(byte[,] ImageMatrix, int Ws, int Wmax, PictureBox PicBox, String sortType)
        {
            int new_i, new_j;//O(1)
            

            if (Wmax % 2 == 0 )//O(1) // check window max size
            {
                Wmax++;//O(1)
            }

            //i-->rows
            for (int i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++)//O(height)
            {
                //j-->cols
                for (int j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++)//O(width)
                {
                    new_i = i + (Ws - 1);//O(1)
                    new_j = j + (Ws - 1);//O(1)
                    //window fit i,j
                    if (new_i < ImageOperations.GetHeight(ImageMatrix) && new_j < ImageOperations.GetWidth(ImageMatrix))//O(1)
                    {
                        int centeral_i = i + (Ws / 2);//O(1)//center of each window
                        int centeral_j = j + (Ws / 2);//O(1)

                        //get window
                        List<byte> current_window = new List<byte>();//O(1)
                        current_window = ImageOperations.check(ImageMatrix, Ws, i, j);//O(Ws^2)

                        //Sorting
                        // Convert list current_window into array 
                        byte[] New_Arr = current_window.ToArray();//O(N)
                       
                        
                        ////-------------------------------------------COUNTING SORT------------------------------
                        if (sortType == "countingSort")
                        {
                            current_window = COUNTING_SORT(New_Arr);//O(N^2)
                           // current_window.Sort();
                        }
                        if (sortType == "Quick_Sort")
                        {
                            ////--------------------------------------------QUICK SORT---------------------------------
                             current_window = Quick_Sort(New_Arr, 0, New_Arr.Length - 1);//O(N^3)
                            //current_window.Sort();
                        }

                        //Step1
                        int Zmax, Zmin, Zmed, Zxy, A1, A2, B1, B2;//O(1)
                        Zmax = current_window[current_window.Count - 1];//O(1)
                        Zmin = current_window[0];//O(1)
                        Zmed = current_window[current_window.Count / 2];//O(1)
                        A1 = Zmed - Zmin;//O(1)
                        A2 = Zmax - Zmed;//O(1)

                        //loop while med is not valid
                        bool Ws_is_max = false;//to begin in step2
                        while (!(A1 > 0 && A2 > 0))//O(Ws)
                        {

                            Ws = Ws + 2;//O(1)
                            if (Ws > Wmax)//O(1)
                            {
                                ImageMatrix[centeral_i, centeral_j] = (byte)Zmed;//O(1)
                                Ws_is_max = true;//O(1)
                                break;
                            }
                            else
                            {
                                //repeat Step1
                                current_window = ImageOperations.check(ImageMatrix, Ws, i, j);//O(1)
                                Zmax = current_window[current_window.Count - 1];//O(1)
                                Zmin = current_window[0];//O(1)
                                Zmed = current_window[current_window.Count / 2];//O(1)
                                A1 = Zmed - Zmin;//O(1)
                                A2 = Zmax - Zmed;//O(1)

                            }

                        }

                        //step2 if median is valid  when while condition is false
                        if (Ws_is_max == false)//O(1)
                        {
                            Zxy = ImageMatrix[centeral_i, centeral_j];//O(1) // check if there is a noise or not
                            B1 = Zxy - Zmin;//O(1)
                            B2 = Zmax - Zxy;//O(1)

                            if (B1 > 0 && B2 > 0)
                                //centeral of window according to whole matrix of image are same locations
                                ImageMatrix[centeral_i, centeral_j] = (byte)Zxy;//O(1) //no noise
                            else
                                ImageMatrix[centeral_i, centeral_j] = (byte)Zmed;//O(1) //noise

                        }

                    }

                }
            }
            ImageOperations.DisplayImage(ImageMatrix, PicBox);
        }//O(N^7)

        public static void alpha(byte[,] ImageMatrix, int Ws, PictureBox PicBox, String sortType, int Trim_Val)
        {
            int new_i, new_j;//O(1)
            
            if (Ws < 3)//O(1)
            {
                Ws = 3;//O(1)
            }
            if (Ws % 2 == 0 && Ws >= 3)//O(1) // check window size
            {
                Ws++;//O(1)
            }

            //i-->rows
            for (int i = 0; i < ImageOperations.GetHeight(ImageMatrix); i++)//O(height)
            {
                //j-->cols
                for (int j = 0; j < ImageOperations.GetWidth(ImageMatrix); j++)//O(height)
                {
                    int avg = 0;//O(1)
                    int sum = 0;//O(1)
                    new_i = i + (Ws - 1);//O(1)
                    new_j = j + (Ws - 1);//O(1)
                    //window fit i,j (in image matrix)
                    if (new_i < ImageOperations.GetHeight(ImageMatrix) && new_j < ImageOperations.GetWidth(ImageMatrix))//O(1)
                    {
                        int centeral_i = i + (Ws / 2);//O(1) //center of each window
                        int centeral_j = j + (Ws / 2);//O(1)

                        //get window
                        List<byte> current_window = new List<byte>();//O(1)
                        current_window = ImageOperations.check(ImageMatrix, Ws, i, j);//O(Ws^2)

                        //Sorting
                        // current_window.Sort();
                        // Convert list current_window into array 
                        byte[] New_Arr = current_window.ToArray();//O(N)
                      
                        // current_window.Sort();
                        //----------------------------------COUNTING SORT-------------------------------------------------------------
                        if (sortType == "countingSort")//O(1)
                        {

                            current_window = COUNTING_SORT(New_Arr);//O(N^2)
                            //cutting biggset&smallest
                            if (Trim_Val >= 1 && Trim_Val <= current_window.Count)//O(1)
                            {
                                current_window.RemoveAt(Trim_Val - 1);//O(1)
                                current_window.RemoveAt(current_window.Count - Trim_Val);//O(1)
                            }

                        }//O(n^2)
                        else
                        {

                           ////-----------------------------------kTH ELEMENT----------------------------------------------------------------

                            if (Trim_Val == 1)//O(1)
                            {
                                byte max_kth = Kthsmalest(New_Arr, New_Arr.Length);//O(N^2((LogN))
                                byte min_kth = Kthsmalest(New_Arr, Trim_Val);//O(N^2((LogN))
                                current_window.Remove(max_kth);//O(1)
                                current_window.Remove(min_kth);//O(1)
                            }//O((N^2)(LogN)))

                            else if (Trim_Val > 1 && Trim_Val <= current_window.Count)
                            {
                                byte max_kth = Kthsmalest(New_Arr, New_Arr.Length - (Trim_Val - 1));//O(N^2((LogN))
                                byte min_kth = Kthsmalest(New_Arr, Trim_Val);//O(N^2((LogN))
                                current_window.Remove(max_kth);//O(1)
                                current_window.Remove(min_kth);//O(1)
                            }//O((N^2)(LogN)))

                        }

                        //get sum after
                        for (int k = 0; k < current_window.Count; k++)//O(Ws)
                            sum += current_window[k];//O(1)
                        //get avg
                        avg = sum / current_window.Count;//O(1)
                        //assign avg to centeral
                        ImageMatrix[centeral_i, centeral_j] = (byte)avg;//O(1)
                    }
                    
                }

            }
            // Display it using Imageoperation.DisplayImage()
            ImageOperations.DisplayImage(ImageMatrix, PicBox);
        }
    }//O(N^8)
}
