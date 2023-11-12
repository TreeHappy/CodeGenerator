var parameters = Environment.GetCommandLineArgs();
var csvFile = parameters[1];
var outputFile = parameters[2];

generateCode(csvFile, outputFile);

static void generateCode(string inputFile, string outputFile)
{
    Console.WriteLine($"in {inputFile}");
    Console.WriteLine($"out {outputFile}");

    var inputFileInfo = new FileInfo(inputFile);
    var outputFileInfo = new FileInfo(outputFile);

    Console.WriteLine($"Generating coddde for {inputFileInfo.Name} into {outputFileInfo.Name}.");

    var headers =
      System.IO.File
      .ReadLines(inputFile)
      .First()
      .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var records =
      headers
      .Select(header => $"public record Header{header}();")
      .ToArray();
    var cellTypes =
      headers
      .Select(header => $"public record Cell{header}(string Data);")
      .ToArray();
    var cellTypeParameterList =
      headers
      .Select(header => $"Cell{header}[] {header}Cells");
    var cellTypeParameterListString =
      string.Join(", ", cellTypeParameterList);
    var parameters =
      headers
      .Select(header => $"Header{header} {header}Header");
    var parameterString =
      string.Join(", ", parameters);
    var fileNameNoExtension =
      outputFileInfo.Name.Split(".").First();

    System.Console.WriteLine(fileNameNoExtension);

    var stringBuilder = new System.Text.StringBuilder();

    stringBuilder.AppendLine($"namespace library.{fileNameNoExtension};");
    stringBuilder.AppendLine();
    stringBuilder.AppendLine($"public record CsvColumnMajor ({parameterString}, {cellTypeParameterListString});");
    stringBuilder.AppendLine();
    stringBuilder.AppendJoin(Environment.NewLine, cellTypes);
    stringBuilder.AppendLine();
    stringBuilder.AppendJoin(Environment.NewLine, records);

    File.WriteAllText(outputFile, stringBuilder.ToString());
}

