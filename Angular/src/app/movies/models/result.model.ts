import { TMDB } from "./tmdb.model";

export class Result {
    page!:number;
    results!:TMDB[];
    total_pages!:number;
    total_results!:number;
}