
using FolderComparer;
using System.Collections.Generic;
using System.Diagnostics;

string firstFolder = string.Empty, secondFolder = string.Empty;
Dictionary<string, string> firstFolderEntries = new Dictionary<string, string>();
Dictionary<string, string> secondFolderEntries = new Dictionary<string, string>();
Dictionary<string, string> missingEntries = new Dictionary<string, string>();


run();
void run() {
	ConsoleWriter.Write("Starting folder checker...", true, "green", "black");
	ConsoleWriter.Write("Made by ZuluNiner", true, "cyan", "black");

	//Collect first folder
	collectFolder("first");
	while (!Directory.Exists(firstFolder))
	{
		ConsoleWriter.Write("The requested directory (" + firstFolder + ") doesn't exist",true,"red","white");
		collectFolder("first");
	}
	ConsoleWriter.Write("The first directory has been collected (" + firstFolder + ")");
	
	//Collect second folder
	collectFolder("second");
	while (!Directory.Exists(secondFolder))
	{
		ConsoleWriter.Write("The requested directory (" + secondFolder + ") doesn't exist", true, "red", "white");
		collectFolder("second");
	}
	ConsoleWriter.Write("The second folder has been collected (" + secondFolder + ")");

	ConsoleWriter.Blank();
	ConsoleWriter.Write("Processing folders...", true, "green");

	//Process first folder files
	string[] files = Directory.GetFiles(firstFolder);
	processFirstFolderFiles(files);
	ConsoleWriter.Blank();

	//Process first folder directories
	string[] folders = Directory.GetDirectories(firstFolder);
	processFirstFolderDirectories(folders);
	ConsoleWriter.Blank();

	//Process second folder files and perform first compare
	ConsoleWriter.Write("Starting compare...", true, "cyan", "black");
	files = Directory.GetFiles(secondFolder);
	secondFolderEntries = processFileDifference(files, "Second", firstFolderEntries);
	ConsoleWriter.Blank();

	//Process second folder directories and perform first compare
	folders = Directory.GetDirectories(secondFolder);
	processDirectoryDifference(folders, "Second", firstFolderEntries);
	ConsoleWriter.Blank();

	//Cross compare second folder to first folder
	ConsoleWriter.Write("Starting cross compare...", true, "cyan", "black");
	processFileDifference(firstFolderEntries.Keys.ToArray(),"First",secondFolderEntries);
	ConsoleWriter.Blank();

	ConsoleWriter.Write("Finished cross compare", true, "green", "black");
	if (missingEntries.Count <= 0)
		ConsoleWriter.Write("No missing entries found", true, "green");
	else
		ConsoleWriter.Write("Total missing entries: " + missingEntries.Count,true,"yellow");

	if (File.Exists(Environment.CurrentDirectory + "\\results.txt"))
		File.Delete(Environment.CurrentDirectory + "\\results.txt");
	string result = "SCAN OF (" + firstFolder + ") and (" + secondFolder + ")" + Environment.NewLine;
	foreach(KeyValuePair<string,string> entry in missingEntries)
	{
		result += "Missing " + entry.Value + " - " + entry.Key + Environment.NewLine;
	}
	File.WriteAllText(Environment.CurrentDirectory + "\\results.txt", result);
	ConsoleWriter.Write("Process finished",true,"green","black");
	ConsoleWriter.Write("Results saved to " + Environment.CurrentDirectory + "\\results.txt");
	Console.Write("Press any key to exit...");
	Console.ReadKey();
	Environment.Exit(0);
}

void processFirstFolderDirectories(string[] folders)
{
	ConsoleWriter.Write("Processing " + folders.Length + " directories in the first folder", true, "cyan", "black");
	int processed = 1;
	foreach (string folder in folders)
	{
		ConsoleWriter.Rewrite("Processing " + processed + "/" + folders.Length + " (" + Path.GetFileName(folder) + ")");
		firstFolderEntries.Add(Path.GetFileName(folder), "Directory");
		processed++;
	}
	ConsoleWriter.Rewrite("Processed " + (processed - 1) + "/" + folders.Length + " entries                                  ", true);
	ConsoleWriter.Write("Finished processing first folder directories", true, "green", "black");
}

void processFirstFolderFiles(string[] files)
{
	ConsoleWriter.Write("Processing " + files.Length + " files in first folder", true, "cyan", "black");
	int processed = 1;
	foreach (string file in files)
	{
		ConsoleWriter.Rewrite("Processing " + processed + "/" + files.Length + " (" + Path.GetFileName(file) + ")");
		firstFolderEntries.Add(Path.GetFileName(file), "File");
		processed++;
	}
	ConsoleWriter.Rewrite("Processed " + (processed - 1) + "/" + files.Length + " entries                                  ", true);
	ConsoleWriter.Write("Finished processing files", true, "green", "black");
}

Dictionary<string,string> processDirectoryDifference(string[] folders, string folderCount, Dictionary<string,string> compare)
{
	ConsoleWriter.Rewrite("Processing " + folders.Length + " directories in " + folderCount + " folder", true, "cyan", "black");
	Dictionary<string, string> result = new Dictionary<string, string>();
	int processed = 1;
	int preCheck = missingEntries.Count;
	foreach (string folder in folders)
	{
		result.Add(Path.GetFileName(folder), folderCount + " folder directory");
		ConsoleWriter.Rewrite("Processing " + processed + "/" + folders.Length + " (" + Path.GetFileName(folder) + ")");
		secondFolderEntries.Add(Path.GetFileName(folder), "Directory");
		if (!compare.ContainsKey(Path.GetFileName(folder)))
		{
			//ConsoleWriter.Write(Path.GetFileName(folder) + " was not found");
			missingEntries.Add(Path.GetFileName(folder), folderCount+" folder Directory");
		}
		//else
			//ConsoleWriter.Write("Found: " + Path.GetFileName(folder));
		processed++;
	}
	ConsoleWriter.Rewrite("Processed " + (processed-1) + "/" + folders.Length + " entries                                  ", true);
	if (missingEntries.Count - preCheck <= 0)
	{
		ConsoleWriter.Write("Finished directory compare, found " + (missingEntries.Count - preCheck) + " missing files", true, "green");
	}
	else
	{
		ConsoleWriter.Write("Finished directory compare, found " + (missingEntries.Count - preCheck) + " missing files", true, "yellow");
	}
	return result;
}

Dictionary<string,string> processFileDifference(string[] files, string folderCount, Dictionary<string,string> compare)
{
	Dictionary<string,string> result = new Dictionary<string,string>();
	int processed = 1;
	int preCheck = missingEntries.Count;
	foreach (string file in files)
	{
		result.Add(Path.GetFileName(file), folderCount + " folder file");
		ConsoleWriter.Rewrite("Processing " + processed + "/" + files.Length + " (" + Path.GetFileName(file) + ")");
		if (!compare.ContainsKey(Path.GetFileName(file)))
		{
			//ConsoleWriter.Write(Path.GetFileName(file) + " was not found");
			missingEntries.Add(Path.GetFileName(file), "Second folder File");
		}
		//else
			//ConsoleWriter.Write("Found: " + Path.GetFileName(file));
		processed++;
	}
	ConsoleWriter.Rewrite("Processed " + (processed-1) + "/" + files.Length + " entries                                  ", true);
	if (missingEntries.Count - preCheck <= 0)
	{
		ConsoleWriter.Write("Finished file compare, found " + (missingEntries.Count - preCheck) + " missing files", true, "green");
	}
	else
	{
		ConsoleWriter.Write("Finished file compare, found " + (missingEntries.Count - preCheck) + " missing files", true, "yellow");
	}
	return result;
}

void collectFolder(string number)
{
	switch (number){
		case "first":
			ConsoleWriter.Write("Enter the first folder to scan: ", false, "white", "black");
			firstFolder = Console.ReadLine();
			break;
		case "second":
			ConsoleWriter.Write("Enter the second folder to scan: ", false, "white", "black");
			secondFolder = Console.ReadLine();
			break;
	}
}