package test;

import java.io.File;

import com.ibm.as400.access.FTP;

public class FTPTest {

	public static void main(String[] args) throws Exception {
		FTP ftp = new FTP("172.17.120.98", "mwadmin", "testrite");
		ftp.connect();
		String[] dirArray = { "mwdata", "ftp", "HLC", "outbound" };

		for (String str : dirArray) {
			ftp.cd(str);
		}
		System.out.println("当前目录" + ftp.getCurrentDirectory());
		ftp.put(new File("D:\\test1111.csv"), "alvin.csv");
		ftp.disconnect();
	}
}
