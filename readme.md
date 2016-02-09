## QSSL Tool
A Qualys SSL Checker-Client for non-commercial mass queries.

**Requirements:**    
.NET Framework 4.5.2

**Content**
1. [How to format your Excel files](#format-excel-files)
2. ??

## Format Excel files
The QSSL TOOL works with `.xls` and `.xlsx`-Excel files. In order to make sure that your Excel spread sheet gets parsed properly, you need to label your header columns (first row columns) with following containing key words:
- *IP*, this must be written in upper case.
- *URL*, again case-sensitive.
- *TLS*, again case-sensitive.
- *RC4*, again case-sensitive.
- *MD5*, again case-sensitive.
- *Ranking*, not case-sensitive.
- *Protocol*, not case-sensitive.
- *Fingerprint*, not case-sensitive.
- *Expiration*, not case-sensitive.
