package test;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.List;

public class SqlTest {

	public static void main(String[] args) {
		// foo4("11101","NTH","金四季店","E");
		// foo4("11102","NTH","来广营店","E");
		// foo4("11103","NTH","龙德店","E");
		// foo4("11104","NTH","华腾店","E");
		// foo4("11105","NTH","永旺店","E");
		// foo4("11106","NTH","北京东直门店","E");
		// foo4("11106","NTH","朝阳大悦城店","E");
		// foo4("11109","NTH","中关村欧美汇店","E");

		// 编号 table编号 层编号 列名称 列序号
		int start = 316;
		List<String> params = read();
		for (int i = 0; i < params.size(); i++) {
			foo3(start++ + "", "2", "H1", params.get(i), i + 1 + "");
		}
		// foo1("HHT_A5MTHT", "HHT- STORE资料下载(MASTERDOWNLOAD)", "JA5MT4ALLS");

		// foo2("3", "HHT_SKUTHT", "HTOA1F");
		// foo2("4", "HHT_UPRTHT", "HTOA3F");
		// foo2("5", "HHT_A1MTHT", "HTOA1F");
		// foo2("6", "HHT_B1FTHT", "HTOB1F");
		// foo2("7", "HHT_C1FTHT", "HTOC1F");
		// foo2("8", "HHT_C5FTHT", "HTOC5F");
		// foo2("9", "HHT_D1D2HT", "HTOD1F/HTOD2F");
		// foo2("10", "HHT_STRTHT", "HTOA5F");
		// foo2("11", "HHT_WNDTHT", "HTOA4F");
		// foo2("12", "HHT_APTHHT", "HTOA7F");
		// foo2("13", "HHT_A3MTHT", "HTOA3F");
		// foo2("14", "HHT_A4MTHT", "HTOA4F");
		// foo2("15", "HHT_A5MTHT", "HTOA5F");

	}

	private static List<String> read() {
		File file = new File("D:\\test.txt");
		List<String> list = new ArrayList<String>();
		try {
			BufferedReader reader = new BufferedReader(new FileReader(file));
			String line;
			while ((line = reader.readLine()) != null) {
				list.add(line);
			}
			reader.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return list;
	}

	static void foo4(String... params) {
		String sql = "insert into HOLA_APP_CFG_STOREINFO (STORENO,AREACODE,STRNAME,STATUS)values('"
				+ params[0]
				+ "','"
				+ params[1]
				+ "','"
				+ params[2]
				+ "','"
				+ params[3] + "');";
		System.out.println(sql);
	}

	// 编号 table编号 层编号 列名称 列序号
	static void foo3(String... params) {
		String sql = "insert into MW.HOLA_APP_CFG_CHGTBLDTL "
				+ "(ID, TBLID, SRCLAYCODE, SRCFLDCODE, SRCSEQ, TARLAYCODE, TARFLDCODE, TARSEQ) "
				+ "values ('" + params[0] + "', '" + params[1] + "'," + " '"
				+ params[2] + "', '" + params[3] + "', '" + params[4]
				+ "', null, null, null);";

		System.out.println(sql);

	}

	// // 交换代码 交换名称 电文代码 创建时间
	// static void foo1(String... arrray) {
	// String sql =
	// "insert into MW.HOLA_APP_CFG_CHGINFO (SYSCODE, CHGCODE, CHGNAME, CHGFQ, CHGFU, STARTTIME, IOTYPE, CHGTYPE, CHGMED, SRCCODE, MSGCODE, MSGSEQTYP, SEQCODE, SEQLEG, MSGFORMAT, CURSTATUS, STATUS, MQIP, MQUSERNAME, MQPWD, QMGNAME, QMGPORT, OQNAME, IQNAME, QCCSID, FTPURL, FTPUSERNAME, FTPPWD, TARDBURL, TARDBDRIVER, TARDBSCHEMA, TARDBUSERNAME, TARDBPWD, REMARK, CREATETIME)"
	// + " values ('JDA', '"
	// + arrray[0]
	// + "', '"
	// + arrray[1]
	// + "', "
	// + "null, null, null, 'O', '1', 'F', 'HLC', "
	// + "'"
	// + arrray[2]
	// + "', 'C', 'TAB', null, 'U', 'Y', 'E',"
	// + " null, null, null, null, null, null, null, null, "
	// + "'ftp://172.17.120.98/mwdata/ftp/HLC/outbound/',"
	// + " 'mwadmin', 'testrite', null, null, null, null, null, null, null)";
	//
	// System.out.println(sql);
	// }

	// static void foo2(String... params) {
	// String sql =
	// "insert into MW.HOLA_APP_CFG_CHGTBL (ID, CHGCODE, SRCTBLNAME, TARTBLNAME) values "
	// + "('"
	// + params[0]
	// + "', '"
	// + params[1]
	// + "', '"
	// + params[2]
	// + "', null);";
	// System.out.println(sql);
	// }

}
