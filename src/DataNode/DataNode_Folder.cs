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
    }

    public string FolderName { get; }
    public List<DataNode_Folder> Folders { get; } = new();
    public List<Lazy<DataNode>> Files { get; } = new();

    public override string TypeDisplayName => "Folder";
    public override string DisplayName => FolderName == String.Empty ? "ROOT" : FolderName;
    public override bool HasChildren => true;
    public override GenericIconKind IconKind => GenericIconKind.DataNode_Folder;

    public static DataNode_Folder FromTypedFiles<TFile>(
        IEnumerable<TFile> files,
        Func<TFile, string> getFilePathFunc,
        Func<TFile, Lazy<DataNode>> createFileNodeFunc)
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
            prevItem.Files.Add(createFileNodeFunc(file));
        }

        return root;
    }

    public static DataNode_Folder FromUntypedFiles<TFile>(
        IEnumerable<TFile> files,
        FileContext fileContext,
        Func<TFile, string> getFilePathFunc,
        Func<TFile, Stream> getFileStreamFunc)
    {
        return FromTypedFiles(files, getFilePathFunc, x =>
        {
            string filePath = getFilePathFunc(x);

            // Attempt to find a matching file type
            IFileType? type = FileTypes.FindFileType(filePath);

            // Use a normal file node if none was found
            if (type == null)
                return new Lazy<DataNode>(() => new DataNode_File(Path.GetFileName(getFilePathFunc(x))));

            return new Lazy<DataNode>(() => type.CreateDataNode(fileContext with
            {
                FilePath = filePath,
                FileStream = getFileStreamFunc(x),
            }));
        });
    }

    public override IEnumerable<DataNode> CreateChildren()
    {
        foreach (DataNode_Folder folder in Folders.OrderBy(x => x.DisplayName))
            yield return folder;

        foreach (DataNode file in Files.Select(x => x.Value).OrderBy(x => x.DisplayName))
            yield return file;
    }
}