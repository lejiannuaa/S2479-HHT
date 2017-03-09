package com.hola.bs.print;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.Properties;

public class PrintConfigUtil {
//    private static Configuration config = new ConfigUtils("print_config.xml").geConfig();
    private static Properties props=new Properties();
    private static PrintConfigUtil pc = new PrintConfigUtil();
	
    private PrintConfigUtil(){
		
		try {
			File f=new File(this.getClass().getResource("/").getFile()+"/print_config.xml");
//		File f = new File(System.getProperty("user.dir")+"\\sql.properties");
			System.out.println(f.exists());
			FileInputStream fis  = new FileInputStream(f.getPath());;
//		try {
//			fis = new FileInputStream(str);
//		props.load(System.class.getClass().getResourceAsStream(System.class.getClass().getResource("/")+"/com/hola/contract/resources/config.properties"));
			props.load(fis);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

    public static String getPrinter1Name(){
        return props.getProperty("printer1Name");
    }

    public static String getPrinter2Name(){
        return props.getProperty("printer2Name");
    }
    
    public static String getTemplate1Name(){
        return props.getProperty("template1Name");
    }
    public static String getTemplate2Name(){
        return props.getProperty("template2Name");
    }
    public static String getTemplate3Name(){
        return props.getProperty("template3Name");
    }
    public static String getTemplate4Name(){
        return props.getProperty("template4Name");
    }
    public static String getTemplate5Name(){
        return props.getProperty("template5Name");
    }
    public static String getTemplate6Name(){
        return props.getProperty("template6Name");
    }
    public static String getTemplate7Name(){
    	return props.getProperty("template7Name");
    }
    public static String getTemplate8Name(){
    	return props.getProperty("template8Name");
    }
    public static String getTemplate9Name(){
    	return props.getProperty("template9Name");
    }
    public static String getTemplate10Name(){
    	return props.getProperty("template10Name");
    }
    public static String getTemplate11Name(){
    	return props.getProperty("template11Name");
    }
    public static String getTemplate12Name(){
    	return props.getProperty("template12Name");
    }
    public static String getTemplate13Name(){
    	return props.getProperty("template13Name");
    }
    public static String getTemplate1PrinterName(){
        return props.getProperty("template1PrinterName");
    }
    public static String getTemplate2PrinterName(){
        return props.getProperty("template2PrinterName");
    }
    public static String getTemplate3PrinterName(){
        return props.getProperty("template3PrinterName");
    }
    public static String getTemplate4PrinterName(){
        return props.getProperty("template4PrinterName");
    }    
    public static String getTemplate5PrinterName(){
        return props.getProperty("template5PrinterName");
    }
    public static String getTemplate6PrinterName(){
        return props.getProperty("template6PrinterName");
    }
    public static String getTemplate7PrinterName(){
    	return props.getProperty("template7PrinterName");
    }
    public static String getTemplate8PrinterName(){
    	return props.getProperty("template8PrinterName");
    }
    public static String getTemplate9PrinterName(){
    	return props.getProperty("template9PrinterName");
    }
    public static String getTemplate10PrinterName(){
    	return props.getProperty("template10PrinterName");
    }
    public static String getTemplate11PrinterName(){
    	return props.getProperty("template11PrinterName");
    }
    public static String getTemplate12PrinterName(){
    	return props.getProperty("template12PrinterName");
    }
    private static final String prefix = "hht";
    
    public static String getTemplate1headerSql(String storeid){
        return props.getProperty("template1headerSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate1headerSql2(String storeid){
        return props.getProperty("template1headerSql2").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate1detailSql(String storeid){
        return props.getProperty("template1detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate1TotalDifSql(String storeid){
    	return props.getProperty("template1totalDifSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate2detailSql(String storeid){
        return props.getProperty("template2detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate2TotalSql(String storeid){
    	return props.getProperty("template2TotalSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate3headerSql1(String storeid){
        return props.getProperty("template3headerSql1").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate3headerSql2(String storeid){
        return props.getProperty("template3headerSql2").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate3detailSql(String storeid){
        return props.getProperty("template3detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate3DifTotalSql(String storeid){
    	return props.getProperty("templdate3DifTotalSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate4detailSql(String storeid){
        return props.getProperty("template4detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate4TotalSql(String storeid) {
		return props.getProperty("template4TotalSql").replaceAll("schema", prefix+storeid);
	}

    
    public static String getTemplate5headerSql(String storeid){
        return props.getProperty("template5headerSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate5detailSql(String storeid){
        return props.getProperty("template5detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate5TotalSql(String storeid){
        return props.getProperty("template5totalSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate6headerSql(String storeid){
        return props.getProperty("template6headerSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate6detailSql(String storeid){
        return props.getProperty("template6detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate6TotalSql(String storeid) {
		return props.getProperty("template6TotalSql").replaceAll("schema", prefix+storeid);
	}
    
    public static String getTemplate7headerSql(String storeid) {
        return props.getProperty("template7headerSql").replaceAll("schema", prefix+storeid);
    }

    public static String getTemplate7detailSql(String storeid) {
        return props.getProperty("template7detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate8headerRtvSql(String storeid){
    	return props.getProperty("template8headerRtvSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate8headerTrfSql(String storeid){
    	return props.getProperty("template8headerTrfSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate9detailSql(String storeid){
    	return props.getProperty("template9detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate10headerSql(String storeid){
    	return props.getProperty("template10headerSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate10detailSql(String storeid){
    	return props.getProperty("template10detailSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate11headerSql(String storeid){
    	return props.getProperty("template11headerSql").replaceAll("schema", prefix+storeid);
    }
    
    public static String getTemplate12detailSql(String storeid){
    	return props.getProperty("template12detailSql").replaceAll("schema", prefix+storeid);
    }

}
