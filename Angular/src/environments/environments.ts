export const environment = {
    production: false, 
    apiUrl: 'http://localhost:4654',
    movieUrl: 'https://api.themoviedb.org/3/search/movie?api_key=857013156f2fa06d7ada9a9d6e51896d&page=1&query=',
    posterUrl: "https://image.tmdb.org/t/p/original"
};

// https://api.themoviedb.org/3/movie/<<ID>>/credits?api_key=857013156f2fa06d7ada9a9d6e51896d

// fetch(`https://api.themoviedb.org/3/movie/<<movieID>>/credits?api_key=<<your_api_key>>`)
// .then(response => response.json())
// .then((jsonData)=>jsonData.crew.filter(({job})=> job ==='Director'))