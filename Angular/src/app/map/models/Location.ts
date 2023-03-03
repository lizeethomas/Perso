import { Picture } from "./Picture";

export class Location {
    id!:number;
    name!:string;
    latitude!:number;
    longitude!:number;
    country?:string;
    city?:string;
    pictures?:Picture[];
}