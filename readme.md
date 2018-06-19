## QSSL Tool
An automated Qualys SSL Checker-Client (vulnerability scanner) for non-commercial mass queries.

**Requirements**    
.NET Framework 4.5.2

### > Developer manual
_Last edit on 19.06.2018_
This manual will go through the most important parts of the code.

### 00. The main class

All initial operations and event handlers happen at `MainWindow.xaml.cs`.
The constructor calls all methods to prepare the workflow. To get to know the code, I suggest starting there.

### 01. The API

I have downloaded the API lib from the official repo for C# [from here](https://ashleypoole.co.uk/ssllwrapper/)
and edited it to make use of the newest features of the original API from Qualys itself.

***Example: Adding the Bleichenbacher vuln. test***
First I got at raw view of the API respnse [per HTTP](https://api.ssllabs.com/api/v3/analyze?host=www.google.com&all=on).
There I saw that each endpoint now has a attribute called 'Bleichenbacher' at the Details-Object.

So I extended the locally forked repo's `Details.cs` to contain a getter/setter for that field.
The API library is mainly just classes of getters/setters so the only "hard task" is to find out where things are.

### 02. The ParserDelegator

When using the software, the user is able to parse an already existing file (for instance an Excel-File).
Once the click event handler `OpenFileButtonClick()` is triggered it just delegates the filename to the `ParserDelegator`.

The `ParserDelegator` in turn checks for the file extension. If it is .xls the `ExcelFileParser` will be initialized
internally. Here, you can extend the delegator for other filetypes such as .csv or .txt.

From the outside the `MainWindow.xaml.cs` will just call `ParserDelegator.GetHostEntries()` and expect a list
of parsed `HostEntry`s. That list will be called once the parsing starts at MainWindow's `StartButtonClick`.

### 03. The ExcelFileParser

This class is inheriting from the `FileParser` class which itself contains basic file op-methods and fields.
Also, this class is making use of the Excel parser [ExcelDataReader](https://github.com/ExcelDataReader/ExcelDataReader).

Basically, this class' `parse()` method will go through the user-opened Excel file and check for the indices of the columns
which are to be re-checked by the next qualys analysis.

After the indices have been gathered, `HostEntry` objects will be created according to the data of the user .xls file.

### 04. HostEntry

This class serves as a container of information around a single endpoint (website) of an analysis.
It also has setters which behave according to the respective values. For instance, `SetForwardSecrecy()` expects
an `int` as the only parameter. Yet the internal value for the "forward secrecy" will be different from the passed `int`.
In this case, it will be human readable string.

The HostEntry-Class also has a method called `CheckDifferences()`, which takes another HostEntry to check
whether attributes between these two objects are different. If an attribute is, it will be prefixed with a human
understandable string, such as _Poodle Vuln.: Changed from A to B_.

### 05. The SSLAnalyzer

This class does the core-operations of the software. First it will take the HostEntries and the APIService itself in its
constructor. After calling its `Start()` method from the outside, the member method `analyze()` 
will be called in a seperate thread to lighten the burden for UI-Animations.

__The analyze() method__
Every HostEntry will go through the loop to be analyzed by its URL. The actual settings of the analyses can be edited
in the settings window, yet attributes like the `_waitInterval` are hard-coded.

After a single analysis in the loop is complete, the member method `extractInfoFromAnalysis` will be called. 
This method will, as the name says, extract all gathered information into a `HostEntry`-Object and add that
newly created `HostEntry`-Object into a list of analyzed entries.


### 06. The analysis workflow


__01. StartButtonClick()__

The user starts the analysis by clicking the start button. This method inside the main class will first get 
the parsed HostEntries from the `ExcelFileParser` (Hint: When analyzing a single URL, `AnalyzeButtonClick()` will be
called) and afterwards start the `SSLAnalyzer` itself.

__02. OnAnalyzeProgressed()__

Every time a single URL has been analyzed, this event handler will be called. Inside this handler,
the UI output as well as internal statistics will be updated.

__03. OnAnalyzeComplete()__

Once the analysis is complete, this event handler will be called to enable the export.


### 07. ExcelWriter

This class is, as the name suggests, responsible for the export of the whole analysis.
It is rendering on a row-by-row basis (see the order of method calls in constructor). Also,
it will color each cell in a row by its value (good or bad).


### > User manual
_Last edit on 24.05.2016_
**Download**

**Content**
1. Quick guide
2. How to format Excel files
3. Tips
4. Example files

### 01. Quick guide

With this tool you can check your website for security according to the standards of the (API) [Qualys SSL Test](https://www.ssllabs.com/ssltest/)

There are two ways to do this:

 **1. Start a single query**
	 You can quickly scan an url for its security attributes this way. Just enter a URL (has to be https) into the search field and hit analyze. The tool will run for approx. 100 seconds (depends how good/bad the website is protected).

![Single analysis screenshot](http://onur.areondev.de/wp-content/uploads/2016/05/QSSLToolSingle.png)

You will see the result in the outcome list. You can also save the result to an Excel file.

 **2. Start a mass query**
 A mass query is equally easy to set up. Please note that you need to preformat your Excel file (see chapter 2).

Once you load in your Excel files (use *open file*) the tool will parse the file and prepare the analysis.
The tool will show you how long the analysis will take (about 100 seconds per URL). The estimation will be recalculated every time an URL has been analyzed.

When running a mass query, the tool will

 1. Check if the values in the spread sheet are correct.
 2. Notify the user about changes that have happened (different grade, expiration, etc.)

Once this is done (65 websites = 1.5 hours, in my case) you can easily export your result into an Excel file.


### 02. How to format Excel files

The QSSL Tool is able to parse `.xls` and `.xlsx`-Excel files.
In order to make sure that your Excel spread sheet gets parsed properly, you need to label your header columns (first row columns) with following containing key words (all case sensitive):

- *IP*
- *URL*
- *Protocol versions*
- *RC4*
- *Ranking*
- *Protocol*
- *Fingerprint*
- *Beast*
- *Forward secrecy*
- *Heartbleed*
- *Signature algorithm*
- *Poodle*
- *Extended validation*
- *OpenSSL CCS*
- *Http server sig*
- *Server host name*
- *Expiration*

### 03. Tips

 1. When making mass queries, the tool will keep values in *custom fields*. Custom fields are those not included in Chapter 02. Example: You have an extra "Responsible person" column, which will not be checked against the Qualys API. The value in that column : row will be kept for the export.
 2. You can re-query an export file.

### 04. Example files
I have prepared an Excel file which you can just load into the tool and let it run. Obviously you can modify this Excel file to your needs and start automated tests that way.

The files are located in the same OneDrive folder as the tool itself is. You can go to the OneDrive folder [with this link](https://onedrive.live.com/redir?resid=141F81505A5B6387!87936&authkey=!AMeFSMDfV0Zo3SQ&ithint=folder%2czip).
