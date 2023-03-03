import { MovieGenres } from "../enums/MovieGenres";

export class Movie {
    id!:number;
    title!:string;
    director!:string;
    date!:number;
    rating!:number;
    commentary!:string;
    url!:string;
    edited!:Date;
    genres!:MovieGenres[];
    editmode!:boolean;
}