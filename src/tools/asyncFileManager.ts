import { resolve } from "path";

export class AsyncFileManager {
    fs : any;

    constructor() {
        this.fs = require('fs');
    }

    statAsync(path : any) : any {
        return new Promise<any>((resolve, reject) => {
            this.fs.stat(path, (err : any, stats : any) => {
                if (err)
                    reject(err);
                else
                    resolve(stats);
              });
        });
    }

    openAsync(path: any, flags: any, mode : number) : Promise<any> {
        return new Promise<any>((resolve, reject) => {
            this.fs.open(path, flags, mode, (err : any, data : any) => {
                if (err) reject(err)
                else resolve(data)
              });
        });
    }

    readAsync(fd: any, buffer: any, offset: any, length: any, position: any) : Promise<any> {
        return new Promise<any>((resolve, reject) => {
            this.fs.read(fd, buffer, offset, length, position, (err : any, data : any) => {
                if (err)
                    reject(err);
                else
                    resolve(data);
              });
        });
    }

    closeAsync(fd: any) : Promise<any> {
        return new Promise<any>((resolve, reject) => {
            this.fs.close(fd, (err : any) => {
                if (err)
                    reject(err);
                else
                    resolve();
              });
        });
    }

    readFileAsync(path : string, opts : any) : Promise<any> {
        return new Promise<any>((resolve, reject) => {
          this.fs.readFile(path, opts, (err : any, data : any) => {
            if (err)
                reject(err);
            else
                resolve(data);
          })
        })
    }

    readDirAsync(dirPath : string) : Promise<string[]> {
        return new Promise<string[]>((resolve, reject) => {
            this.fs.readdir(dirPath, (err : any, files : string[]) => {
                if (err) 
                    reject(err);
                else
                    resolve(files);
            });
        });
    }

}