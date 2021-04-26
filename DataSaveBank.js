var DataObj = class DataHasMap {

  constructor() {
    this.Data = [];
  }

  getAllDataAsList() {
    var data = [];
    for (var place of this.Data) {
      if (place == null) continue;

      for (var obj of place) {
        data.push([obj[0], obj[1]]); //meak  new parameter in the fileToParse Obj
      }

    }
    return data;
  }
  getAllValuesAsList() {
    var data = [];
    for (var place of this.Data) {
      if (place == null) continue;

      for (var obj of place) {
        data.push(obj[1]);            // meak  new parameter in the fileToParse Obj
      }
    }
    return data;
  }

  getAllData() {

    var data = [];

    for (var i = 0; i < this.Data.length; i++) {
      if (this.Data[i]) {
        data.push(this.Data[i]);
      }
    }
    return data;
  }

  checkExistence(placeInData, placeToStartLooking, key) {
    key = String(key);
    if (this.Data[placeInData][placeToStartLooking] && this.Data[placeInData][placeToStartLooking][0] == key) return placeToStartLooking;
    else if (this.Data[placeInData][placeToStartLooking]) return this.checkExistence(placeInData, placeToStartLooking + 1, key);
    else return null;

  }
  addData(key, value, notOverwriting) {
    var place = 0;
    var placeToPutValueInPlace;
    key = String(key);

    for (var i = 0; i < key.length; i++) {
      place += parseInt(key.charCodeAt(i));
    }

    if (!this.Data[place]) this.Data[place] = [
      [key, value]
    ];
    else {
      placeToPutValueInPlace = this.checkExistence(place, 0, key);
      if (placeToPutValueInPlace != null) {
        if (!notOverwriting) {
          console.log(key + " already existes, changing to the new value of " + value);
          this.Data[place][placeToPutValueInPlace][1] = value;

        } else {
          console.log(" "+key + " already existes, but \"notOverwriting\" is true so didn't change to " + value);
          return false;
        }


      } else this.Data[place].push([key, value]);
    }
    return true;


  }

  getData(key) {
    key = String(key);
    var place = 0;
    for (var i = 0; i < key.length; i++) {
      place += parseInt(key.charCodeAt(i));
    }
    if (!this.Data[place]) return null;
    for (var i = 0; i < this.Data[place].length; i++) {
      if (this.Data[place][i][0] == key) return this.Data[place][i][1];
    }
    return null;

  }



  StoreDataInParms() {
    //var parameter = "<input type=\"hidden\" name=\"data\" value=\"";
    var parameter;
    for (var i = 0; i < this.Data.length; i++) {
      if (!this.Data[i]) break;
      for (var j = 0; j < this.Data[i].lenght; j++) {
        parameter += this.Data[i][j][0] + "," + this.Data[i][j][1] + ":";

      }
    }
    document.getElementById("data").value = parameter;
    document.getElementById("form").submit();
    //parameter += "\"/>";
  }

  delete(key) {
    key = String(key);
    var place = 0;
    for (var i = 0; i < key.length; i++) {
      place += parseInt(key.charCodeAt(i));
    }
    if (!this.Data[place]) return null;
    for (var i = 0; i < this.Data[place].length; i++) {
      if (this.Data[place][i][0] == key) this.Data[place].splice(i, 1);
      if (this.Data[place] == null) this.Data.splice(place, 1);
    }

  }

  getJSON() {
    var fileToParse = {

    }

    for (var place of this.Data) {
      if (place == null) continue;

      for (var obj of place) {
        fileToParse[obj[0]] = obj[1]; //meak  new parameter in the fileToParse Obj
      }

    }

    return JSON.stringify(fileToParse);

  }



};

module.exports.DataObj = DataObj

/* an Exaple how the Data stores works
addData("Jannes",16);
addData("aJnnes",15);
addData("Nore",13);
addData("Boaz",11);


console.log("Jannes is "+ getData("Jannes")+" years old,\naJnnes is " + getData("aJnnes")+" years old \n");
console.log("Nore is "+ getData("Nore") +" yeas old\n and Boaz is "+ getData("Boaz")+ " years old!! ");
console.log("But in 2 years wa are: ");


addData("Jannes",getData("Jannes")+2);
addData("aJnnes",getData("aJnnes")+2);
addData("Nore",getData("Nore")+2);
addData("Boaz",getData("Boaz")+2);

console.log("Jannes is "+ getData("Jannes")+" years old,\naJnnes is " + getData("aJnnes")+" years old \n");
console.log("Nore is "+ getData("Nore") +" yeas old\n and Boaz is "+ getData("Boaz")+ " years old!! ");


console.log(Data);*/
