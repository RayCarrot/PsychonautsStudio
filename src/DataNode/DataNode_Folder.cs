using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PsychonautsTools;

public class DataNode_Folder : DataNode
{
    public DataNode_Folder(string folderName)
    {
        FolderName = folderName;
        ViewModel = new FolderEditorViewModel(() => CreateChildren(null));
    }

    private FolderEditorViewModel ViewModel { get; }
    public override EditorViewModel EditorViewModel => ViewModel;

    public string FolderName { get; }
    public List<DataNode_Folder> Folders { get; } = new();
    public List<Lazy<DataNode>> Files { get; } = new();

    public override string TypeDisplayName => "Folder";
    public override string DisplayName => FolderName == String.Empty ? "ROOT" : FolderName;
    public override bool HasChildren => true;

    public static DataNode_Folder FromTypedFiles<TFile>(
        IEnumerable<TFile> files,
        Func<TFile, string> getFilePathFunc,
        Func<TFile, string, Lazy<DataNode>> createFileNodeFunc)
    {
        DataNode_Folder root = new(String.Empty);

        char[] sepChar = { '/', '\\' };

        // Add each directory
        foreach (TFile file in files)
        {
            string filePath = getFilePathFunc(file);
            string[] splitPath = filePath.Trim(sepChar).Split(sepChar);

            // Keep track of the previous item
            DataNode_Folder prevItem = root;

            // Enumerate each sub directory
            foreach (string subDir in splitPath.Take(splitPath.Length - 1))
            {
                DataNode_Folder? subFolder = prevItem.Folders.FirstOrDefault(x => x.FolderName == subDir);

                if (subFolder == null)
                {
                    subFolder = new DataNode_Folder(subDir);
                    prevItem.Folders.Add(subFolder);
                }

                prevItem = subFolder;
            }

            // Add the file
            prevItem.Files.Add(createFileNodeFunc(file, splitPath.Last()));
        }

        return root;
    }

    public static DataNode_Folder FromUntypedFiles<TFile>(
        IEnumerable<TFile> files,
        FileContext fileContext,
        Func<TFile, string> getFilePathFunc,
        Func<TFile, string, Stream> getFileStreamFunc)
    {
        return FromTypedFiles(files, getFilePathFunc, (file, fileName) =>
        {
            // Attempt to find a matching file type
            IFileType? type = FileTypes.FindFileType(getFilePathFunc(file));

            // Use a normal file node if none was found
            if (type == null)
                return new Lazy<DataNode>(() => DataNode_File.FromStream(fileName, getFileStreamFunc(file, fileName)));

            return new Lazy<DataNode>(() =>
            {
                Stream fileStream = getFileStreamFunc(file, fileName);

                try
                {
                    return type.CreateDataNode(fileContext with
                    {
                        FilePath = getFilePathFunc(file),
                        FileStream = fileStream,
                    });
                }
                catch (Exception ex)
                {
                    // TODO: Handle exception. Log? Error message?
                    return DataNode_File.FromStream(fileName, fileStream);
                }
            });
        });
    }

    public override IEnumerable<DataNode> CreateChildren(FileContext? fileContext)
    {
        foreach (DataNode_Folder folder in Folders.OrderBy(x => x.DisplayName))
            yield return folder;

        foreach (DataNode file in Files.Select(x => x.Value).OrderBy(x => x.DisplayName))
            yield return file;
    }
}