Database was taken from https://github.com/Te-k/malware-classification .

Authors extracted features from binaries.
Whole list of extracted features (54):
	Name, md5, Machine, SizeOfOptionalHeader, Characteristics, MajorLinkerVersion, 
	MinorLinkerVersion, SizeOfCode, SizeOfInitializedData, SizeOfUninitializedData, 
	AddressOfEntryPoint, BaseOfCode, BaseOfData, ImageBase, SectionAlignment, 
	FileAlignment, MajorOperatingSystemVersion, MinorOperatingSystemVersion, 
	MajorImageVersion, MinorImageVersion, MajorSubsystemVersion, MinorSubsystemVersion, 
	SizeOfImage, SizeOfHeaders, CheckSum, Subsystem, DllCharacteristics, 
	SizeOfStackReserve, SizeOfStackCommit, SizeOfHeapReserve, SizeOfHeapCommit, 
	LoaderFlags, NumberOfRvaAndSizes, SectionsNb, SectionsMeanEntropy, 
	SectionsMinEntropy, SectionsMaxEntropy, SectionsMeanRawsize, SectionsMinRawsize, 
	SectionMaxRawsize, SectionsMeanVirtualsize, SectionsMinVirtualsize, 
	SectionMaxVirtualsize, ImportsNbDLL, ImportsNb, ImportsNbOrdinal, ExportNb, 
	ResourcesNb, ResourcesMeanEntropy, ResourcesMinEntropy, ResourcesMaxEntropy, 
	ResourcesMeanSize, ResourcesMinSize, ResourcesMaxSize, LoadConfigurationSize, 
	VersionInformationSize.


Database consists of 41323 instances of benign files 
and 96724 instances of malware, downloaded from Virus Share.

For benign files, author gathered all the Windows binaries (exe + dll) from Windows 2008, 
Windows XP and Windows 7 32 and 64 bits. 
Regarding malware, author used a part of Virus Share collection by downloading one archive (the 134th) 
and kept only PE files. He used pefile to extract all these features from the binaries and store them 
in a csv file.



Detailed description of the database can be found on: 
https://www.randhome.io/blog/2016/07/16/machine-learning-for-malware-detection/

