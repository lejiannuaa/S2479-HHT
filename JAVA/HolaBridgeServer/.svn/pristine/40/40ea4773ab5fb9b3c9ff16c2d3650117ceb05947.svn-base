package com.hola.bs.pool;

import edu.emory.mathcs.backport.java.util.concurrent.ThreadFactory;

public class ThreadFactoryImpl implements ThreadFactory{

    public Thread newThread(Runnable r){
        Thread t = new Thread(r);
        t.setDaemon(true);
        return t;
    }
}
