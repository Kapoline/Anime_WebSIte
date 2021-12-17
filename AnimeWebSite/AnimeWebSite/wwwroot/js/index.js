$.ajax({
    type: "GET",
    url: "/Home/OnGetAnimeItems",
    success: function(data) {
        var parsedData = JSON.parse(data)
        parsedData.items.forEach(item => pringData(item));
    }
});

function pringData(item){
    var card = document.createElement("div")
    card.setAttribute('class', 'card item-holder')
    card.setAttribute('id', item.ItemId)
    card.setAttribute('style', 'width: 18rem;')
    var cardBody = document.createElement("div");
    cardBody.setAttribute('class', 'card-body')
    var cardTitle = document.createElement("h4");
    cardTitle.setAttribute('class', 'card-title')
    cardTitle.innerHTML = item.Name;
    var cardText = document.createElement("p");
    cardText.setAttribute('class', 'card-text')
    cardText.innerHTML = item.Description;
    cardText.setAttribute('style', 'background-color = red;')
    var cardYear = document.createElement('p')
    cardYear.setAttribute('class', 'card-text')
    cardYear.innerHTML = "Год выпуска: " + item.Year
    var cardGenre = document.createElement('p')
    cardGenre.setAttribute('class', 'card-text')
    cardGenre.innerHTML = "Жанр: " + item.Genre
    var cardDirector = document.createElement('p')
    cardDirector.setAttribute('class', 'card-text')
    cardDirector.innerHTML = "Режисер: " + item.Director
    var cardSeriesCount = document.createElement('p')
    cardSeriesCount.setAttribute('class', 'card-text')
    cardSeriesCount.innerHTML = "Число серий: " + item.SeriesCount
    var cardDeleteButton = document.createElement('button')
    cardDeleteButton.setAttribute('class', 'btn btn-danger btn-sm')
    cardDeleteButton.setAttribute('type','button')
    cardDeleteButton.innerHTML = 'Удалить'
    cardDeleteButton.addEventListener('click', function (){
        onItemDeleteButtonClick(item.ItemId)
    });

    var cardImgTop = document.createElement("img");
    cardImgTop.setAttribute('class', 'card-img-top card-img')
    cardImgTop.setAttribute('src', item.PosterPath)
    cardBody.appendChild(cardTitle)
    cardBody.appendChild(cardText)
    cardBody.appendChild(cardYear)
    cardBody.appendChild(cardGenre)
    cardBody.appendChild(cardDirector)
    cardBody.appendChild(cardSeriesCount)
    cardBody.appendChild(cardDeleteButton)

    card.appendChild(cardImgTop)
    card.appendChild(cardBody);

    document.getElementById("items-holder").appendChild(card);
}

function onItemDeleteButtonClick(itemId){
    $.ajax({
        type: "POST",
        url: "/Home/OnPostDeleteButton",
        data: {itemId: itemId},
        success: function(data) {
            var card = document.getElementById(itemId);
            document.getElementById('items-holder').removeChild(card)
        }
    });
}