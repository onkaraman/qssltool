## QSSL Tool
A Qualys SSL Checker-Client for non-commercial mass queries.
Last edited on 24.05.2016.

**Requirements**    
.NET Framework 4.5.2

**Content**
1. Quick guide
2. How to format Excel files
3. Tips

###1. Quick guide
With this tool you can check your website for security according to the standards of the (API) [Qualys SSL Test](https://www.ssllabs.com/ssltest/).

There are two ways to do this:

 **1. Start a single query**
	 You can quickly scan an url for its security attributes this way. Just enter a URL (has to be https) into the search field and hit analyze. The tool will run for approx. 100 seconds (depends how good/bad the website is protected).

![Single analysis screenshot](http://onur.areondev.de/wp-content/uploads/2016/05/QSSLToolSingle.png)

You will see the result in the outcome list. You can also save the result to an Excel file.

 **2. Start a mass query**
 A mass query is equally easy to set up. Please note that you need to preformat your Excel file (see chapter 2).

Once you load in your Excel files (use *open file*) the tool will parse the file and prepare the analysis.

The tool will show you how long the analysis will take (about 100 seconds per URL). The estimation will be recalculated every time an URL has been analyzed.

Once this is done (65 websites = 1.5 hours, in my case) you can easily export your result into an Excel file.


###2. How to format Excel files

The QSSL Toolworks with `.xls` and `.xlsx`-Excel files.
In order to make sure that your Excel spread sheet gets parsed properly, you need to label your header columns (first row columns) with following containing key words:

- *IP*, this must be written in upper case.
- *URL*, again case-sensitive.
- *TLS*, again case-sensitive.
- *RC4*, again case-sensitive.
- *MD5*, again case-sensitive.
- *Ranking*, not case-sensitive.
- *Protocol*, not case-sensitive.
- *Fingerprint*, not case-sensitive.
- *Expiration*, not case-sensitive.
