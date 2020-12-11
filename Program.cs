using System;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.IO;
using System.Collections.Generic;
using System.Text;




namespace Zugferd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            string path = @"C:\TEMP\zugferd_2p1_EXTENDED_Warenrechnung.pdf";

            // PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            // PdfObject obj;
            // for (int i = 1; i <= pdfDoc.GetNumberOfPdfObjects(); i++)
            // {
            //     obj = pdfDoc.GetPdfObject(i);
            //     if (obj != null && obj.IsStream())
            //     {
            //         byte[] b;
            //         //try
            //         //{
            //         b = ((PdfStream)obj).GetBytes();
            //         //}
            //         //catch (PdfException exc)
            //         // {
            //         //    b = ((PdfStream)obj).GetBytes(false);
            //         // }

            //         //new FileStream(String.format(DEST, i));
            //         File.WriteAllBytes(@"C:\TEMP\" + i, b);

            //     }
            // }
            // pdfDoc.Close();
            new Program().ExtractAttachments(path);

        }

        internal void ExtractAttachments(string source_name)
        {
            // PdfDictionary documentNames = null;
            // PdfDictionary embeddedFiles = null;
            PdfDictionary fileArray = null;
            PdfDictionary file = null;
            // PRStream stream = null;

            // PdfReader reader = new PdfReader(file_name);
            // PdfDictionary catalog = reader.Catalog;
            
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(source_name));
            PdfDictionary root = pdfDoc.GetCatalog().GetPdfObject();
            PdfDictionary names = root.GetAsDictionary(PdfName.Names);
            PdfDictionary embeddedFilesDic = names.GetAsDictionary(PdfName.EmbeddedFiles);
            

            //documentNames = (PdfDictionary)PdfReader.GetPdfObject(catalog.Get(PdfName.Names));

            if (names != null)
            {
                //embeddedFiles = (PdfDictionary)PdfReader.GetPdfObject(documentNames.Get(PdfName.EmbeddedFiles));
                if (embeddedFilesDic != null)
                {
                    PdfArray embeddedFiles = embeddedFilesDic.GetAsArray(PdfName.Names);
                    int len = embeddedFiles.Size();

                    for (int i = 0; i < len; i+=2)
                    {
                        
                        fileArray = embeddedFiles.GetAsDictionary(i+1);
                        file = fileArray.GetAsDictionary(PdfName.EF);
                        ICollection<PdfName> fileKeys = file.KeySet();
                        foreach (PdfName key in fileKeys)
                        {
                            
                            //stream = (PRStream)PdfReader.GetPdfObject(file.GetAsIndirectObject(key));
                            string attachedFileName = fileArray.GetAsString(key).ToString();
                            byte[] attachedFileBytes = file.GetAsStream(key).GetBytes();

                            System.IO.File.WriteAllBytes(@"C:\TEMP\" + attachedFileName, attachedFileBytes); 
                        }

                    }
                }    
            }
        }
    }
}
