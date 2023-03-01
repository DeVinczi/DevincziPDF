using System.Data;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Microsoft.Data.SqlClient;

public class PdfGenereator
{
    public static void GeneratePDFFromDataTable()
    {
        var document = new Document();

        // Create a PdfWriter to write the document to a file or stream
        PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));

        // Open the document
        document.Open();

        // Retrieve data from the database
        var connectionString = "Data Source=.;Initial Catalog=DBModelPdf;Integrated Security=True;Encrypt=true;TrustServerCertificate=true;";
        var sql =
            @"
            SELECT b.Url, b.BlogName, p.Title, p.Content
            FROM Blogs b
            INNER JOIN Posts p 
            ON b.BlogId = p.BlogId;";

        var dataTable = new DataTable();
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand(sql, connection);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
        }

        // Create a table in the PDF document and add data from the DataTable
        var table = new PdfPTable(dataTable.Columns.Count);
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
            table.AddCell(new PdfPCell(new Phrase(dataTable.Columns[i].ColumnName)));
        }
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                table.AddCell(new PdfPCell(new Phrase(dataTable.Rows[i][j].ToString())));
            }
        }

        document.Add(table);
        //dodawanie
        Paragraph newParagraph = new Paragraph("This is a new paragraph");
        newParagraph.FirstLineIndent = 0;
        document.Add(newParagraph);
        //wcinanie
        Paragraph nike = new Paragraph("Logo Nike");
        nike.FirstLineIndent = 100;
        document.Add(nike);
        //logo
        var imagePath = "nike-logo.png";
        var image = iTextSharp.text.Image.GetInstance(imagePath);

        // skalowanie
        image.ScaleToFit(document.PageSize.Width - 150f, document.PageSize.Height - 150f);
        document.Add(image);

        var fontPath = "C:\\Windows\\Fonts\\arial.ttf";
        var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        // czcionka
        var font = new Font(baseFont, 16, Font.ITALIC);

        // czcionka i tekst
        var phrase = new Phrase("Nike", font);
        document.Add(phrase);

        //centrowanie tekstu
        var paragraph = new Paragraph("This text is centered.");
        paragraph.Alignment = Element.ALIGN_CENTER;
        document.Add(paragraph);

        //zwykly tekst
        var originalParagraph = new Paragraph("Original text");
        document.Add(originalParagraph);

        //klonowanie
        var shallowCopy = originalParagraph.CloneShallow(true) as Phrase;
        shallowCopy.Font = new Font(Font.FontFamily.COURIER, 16, Font.ITALIC, BaseColor.BLUE);

        // oryginalny i sklonowany
        document.Add(shallowCopy);

        //Na czerwono i pogrubiony
        var phrase2 = new Phrase("Hello World", new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD, BaseColor.RED));
        document.Add(phrase2);

        var paragraph22 = new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed faucibus, ex sed finibus maximus, dolor nibh hendrerit orci, eu ullamcorper lorem arcu eget dui. Integer rhoncus nulla quis ligula laoreet, quis ullamcorper magna dictum. Nullam id dui nec est luctus sagittis. Vivamus sit amet orci euismod, mattis risus id, convallis felis. Pellentesque et sollicitudin massa. ");
        paragraph22.Leading = 20f; // Set the leading to 20 points
        document.Add(paragraph22);

        Paragraph p = new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vel enim non eros condimentum vehicula. Vivamus vitae arcu ut nisl pulvinar aliquet ut nec lacus. Duis at mi ut neque ullamcorper suscipit in eu quam.");
        // ustawianie wciecia
        p.FirstLineIndent = 20f;
        document.Add(p);

        var chunk = new Chunk("This is a chunk with leading.", FontFactory.GetFont(FontFactory.HELVETICA, 12));

        document.Add(chunk);

        Chunk chunk2 = new Chunk("Hello, world!", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK));
        document.Add(chunk2);

        //odnosnik XD
        Anchor anchor = new Anchor("Click here to go to Google");
        anchor.Reference = "http://www.google.com";
        document.Add(anchor);


        var record = new { FirstName = "Dawid", LastName = "Slysz", Age = 89 };
        var paragraph44 = new Paragraph();

        // ustawianie czcionki i tekst
        var font1 = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.NORMAL);
        paragraph44.Font = font1;

        // formatowanie tekstu do lewej 
        paragraph44.Alignment = Element.ALIGN_LEFT;

        // tekst
        paragraph44.Add("Imie: ");
        paragraph44.Add(new Chunk(record.FirstName));
        paragraph44.Add(Chunk.TABBING);
        paragraph44.Add("Nazwisko: ");
        paragraph44.Add(new Chunk(record.LastName));
        paragraph44.Add(Chunk.TABBING);
        paragraph44.Add("Wiek: ");
        paragraph44.Add(new Chunk(record.Age.ToString()));

        // Add the paragraph to the document
        document.Add(paragraph44);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var dbsql = @"SELECT Content FROM Posts Where PostId = 1";
            var command = new SqlCommand(dbsql, connection);
            var reader = command.ExecuteReader();

            // przechodzenie przez rekordy w bazie danych i dodawanie ich jako paragraphy
            while (reader.Read())
            {
                var text = reader[0].ToString(); // zamiana na tekst danych z bazy
                var paragraph55 = new Paragraph(text); // tworzenie nowego paragarafu
                document.Add(paragraph55); // dodawanie go na koniec pdfa
            }

            reader.Close();
            document.Close();
        }
    }
}