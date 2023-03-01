var document = new Document();

// Create a PdfWriter to write the document to a file or stream
var writer = PdfWriter.GetInstance(document, new FileStream("output.pdf", FileMode.Create));

// Open the document
document.Open();

// Retrieve data from the database
var connectionString = "your connection string";
var sql = "your sql query";
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

// Add the table to the PDF document
document.Add(table);

// Close the document
document.Close();