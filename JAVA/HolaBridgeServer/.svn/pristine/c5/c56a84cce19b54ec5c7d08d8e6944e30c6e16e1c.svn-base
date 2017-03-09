package com.hola.bs.util;

import org.apache.commons.net.ftp.FTPClient;
import org.apache.commons.net.ftp.FTPConnectionClosedException;
import org.apache.commons.net.ftp.FTPReply;
import org.apache.commons.net.ftp.FTP;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.property.ConfigPropertyUtil;

import java.net.SocketException;
import java.util.ArrayList;
import java.util.List;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.FileInputStream;
import java.io.File;

public class FTPUtil {
    Logger log = Logger.getLogger(FTPUtil.class);

    private FTPClient client = new FTPClient();
    private String server;
    
    public FTPUtil(String host) {
        server=host;
    }
    
    public void connect(String user, String password) throws IOException {
        try {
            log.debug("connecting to ftp server : " + server);
            client.connect(server);
        } catch (SocketException e) {
            throw new IOException("connect refuse " + server, e);
        } catch (IOException e) {
            throw new IOException("connect refuse " + server, e);
        }
        int reply = client.getReplyCode();
        if (!FTPReply.isPositiveCompletion(reply)) {
            throw new IOException("connect refuse " + server);
        }
        log.debug("connected to ftp server : " + server+" ok.");
        login(user,password);
    }

    private void login(String userName, String password) throws IOException {

        boolean success = client.login(userName, password);
        if (!success) {
            log.error("login failed, " + server);
            try {
                client.disconnect();
            } catch (IOException e) {} 
            throw new IOException("login failed, " + server);
        } else {
            log.debug("login in successfully, " + server);
        }
    }

    /**
     * Use ASCII mode for file transfers
     */
    public boolean ascii() throws IOException {
        return client.setFileType(FTP.ASCII_FILE_TYPE);
    }

    /**
     * Use Binary mode for file transfers
     * @throws NetException 
     */
    public void binary() throws IOException {
        boolean success=client.setFileType(FTP.BINARY_FILE_TYPE);
        if (!success) throw new IOException("set transfer mode to bin error");
    }


    public void uploadFile(File file, String serverFile) throws IOException {
        FileInputStream in = null;
        boolean success = false;
        try {
            success=client.deleteFile(file.getName());
//            if (!success) throw new NetException("delete file "+serverFile+" on remote host "+this.server+" error.");
            in = new FileInputStream(file);
            success = client.storeFile(serverFile, in);
            if (!success) throw new IOException(file.getAbsolutePath() + " upload to remote host "+this.server+" error.");
        } finally {
            if (in != null) {
                try {
                    in.close();
                } catch (IOException e) {}
            }
        }

    }
    
    public void downloadFile(File file, String serverFile) throws IOException {
        FileOutputStream out = null;
        boolean result = false;
        try {
            out = new FileOutputStream(file);
            client.enterLocalPassiveMode();
            result = client.retrieveFile(serverFile, out);
            if (!result) throw new IOException("fail to download file "+file.getAbsolutePath()+" from host "+this.server);
        } finally {
            if (out != null) {
                try {
                    out.close();
                } catch (IOException e) {}
            }
        }

    }
    
    public void move(String srcPath,String destPath) throws IOException {
    	boolean result = false;
    	result= client.rename(srcPath, destPath);
    	if (!result) throw new IOException("rename file from "+srcPath+" to "+destPath+" on host "+this.server+" error.");
    }
    
	public void changeWorkingDirectory(String targetDir) throws IOException {
        boolean success = false;
		success = client.changeWorkingDirectory(targetDir);
		if (!success)
			throw new IOException("can not change to dir:" + targetDir
					+ " on host " + this.server);
    }

	public void rename(String srcName,String destName) throws IOException {
		boolean success=false;
		success=client.rename(srcName, destName);
		if (!success)
			throw new IOException("can rename file "+srcName+" to "+destName
					+ " on host " + this.server);
	}

    public void disconnect() {
        try {
            client.disconnect();
        } catch (IOException e) {
            log.error(e);
        }
    }
    
	
	public List listFileNames() throws IOException,
			FTPConnectionClosedException {
		client.enterLocalPassiveMode();
		String[] files = client.listNames();
		System.out.println("----file list length " + files.length);
		List<String> v = new ArrayList<String>();
		for (int i = 0; i < files.length; i++) {
			System.out.println("----file list " + files[i]);
			if (files[i] != null && files[i].endsWith("gz"))
				// v.addElement(new
				// String(files[i].getName().getBytes("ISO-8859-1"),"GBK"));
				System.out.println("is file " + files[i]);
			v.add(files[i]);
		}
		return v;
	}
	
	
    public static void main(String[] args) {
    	FTPUtil ftp = new FTPUtil("172.16.251.144");
    	try {
			ftp.connect("hht","Hht654321");
			System.out.println("ftp connect to 172.16.251.144, sucess.");
			ftp.binary();
			File localImgFile = new File("D:\\HHTIMG\\ca3fb4bd-58dd-488c-8673-be4cb954e993.jpg");
			System.out.println("download file from: "+ "11108/ca3fb4bd-58dd-488c-8673-be4cb954e993.jpg" +" to : D:\\HHTIMG\\ca3fb4bd-58dd-488c-8673-be4cb954e993.jpg");
			ftp.downloadFile(localImgFile, "11108/ca3fb4bd-58dd-488c-8673-be4cb954e993.jpg");
			System.out.println("download sucess.");
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
    }
}