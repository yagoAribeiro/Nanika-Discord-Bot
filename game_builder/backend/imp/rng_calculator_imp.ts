class RngSampleCalculator extends Array<RngSample>{
    ceil: number = 0;
    constructor(...sampleList: RngSample[]){
        super(...sampleList);
        this.updateCollection();
    }
    public rollAndPull(number: number | null) : RngSample | null{
        if(number == null) number = Math.floor(Math.random()*this.ceil);
        number = Math.max(0, number);
        number = Math.min(this.ceil, number);
        let pullIndex : number = this.intervalBinSearch(number, 0, this.length-1);
        this.applyPity(pullIndex);
        return this[pullIndex];
    }
    public sortSamples() : void {
        this.sort((x : RngSample, y : RngSample) => x.event_value > y.event_value ? 1 : x.event_value < y.event_value ? -1 : 0);
    }
    private applyPity(pullIndex: number) : void{
        if((this[pullIndex].reset_ratio??0)>0){
            this.resetBase(this[pullIndex].reset_ratio!);
            return;
        }
        let ratio : number = this.ceil/this[pullIndex].event_value/100;
        for(let i: number = 0; i<pullIndex; i++){
            this[i].event_value *= 1.0+ratio;
            ratio *= 0.8;
        }
        this.updateCollection();
    }

    private updateCollection() : void{
        this.sortSamples();
        this.ceil = 0;
        for(let i: number = 0; i<this.length; i++){
            let sample : RngSample = this[i];
            sample.event_value = Math.floor(sample.event_value);
            sample.interval.x1 = this.ceil+1;
            sample.interval.x2 = this.ceil+sample.event_value;
            this.ceil+=sample.event_value;
        }
    }

    private resetBase(ratio: number){
        for(let i: number = 0; i<this.length; i++){
            this[i].event_value += (this[i].base_value - this[i].event_value)*ratio; //applies the ratio in the difference by the base value from the current value.
        }
        this.updateCollection();
    }

    public seePercentage() : Array<number>{
        let numbers : number[] = [];
        for(let i: number = 0; i<this.length; i++){
            numbers.push(this[i].event_value/this.ceil*100);
        }
        return numbers;
    }

    private intervalBinSearch(n: number, left_index: number, right_index: number) : number{
        let middle_index: number = Math.ceil((left_index+right_index)/2);
        if (middle_index == left_index || middle_index == right_index){
            if(this.checkInterval(n, left_index) == 0) return left_index;
            else return right_index;
        }
        if (this.checkInterval(n, middle_index) >= 1){
            return this.intervalBinSearch(n, middle_index, right_index);
        }else if (this.checkInterval(n, middle_index) <= -1){
            return this.intervalBinSearch(n, left_index, middle_index);
        }else{
            return middle_index;
        }
    }

    private checkInterval(n: number, i: number) : number{
        let interval: {x1: number, x2: number} = this[i].interval;
        return n < interval.x1 ? -1 : n > interval.x2 ? 1 : 0;
    }
}

class RngSample{
    event_value: number = 0;
    base_value: number = 0;
    event_id: number;
    event_name: string;
    interval: {x1: number, x2: number} = {x1: 0, x2: 0};
    reset_ratio: number | null = null

    constructor(event_id: number, event_value: number, event_name: string, reset_ratio: number | null){
        this.event_id = event_id;  
        this.event_value = event_value;
        this.event_name = event_name;
        this.base_value = event_value;
        this.reset_ratio = reset_ratio;
    }

}

export = {RngSampleCalculator, RngSample};
/** 
 
let test = new RngSampleCalculator(
    new RngSample(0, 400000, "Muito Comum", null),
    new RngSample(1, 320000, "Comum", null),
    new RngSample(2, 150000, "Incomum", null),
    new RngSample(3, 78000, "Raro", null),
    new RngSample(4, 34000, "Muito Raro", 0.1),
    new RngSample(5, 11500, "Épico", 0.25),
    new RngSample(6, 5500, "Exótico", 0.5),
    new RngSample(7, 1000, "Lendário", 1));
let index: number = 0;
let pull_id: number = 0;
let pull: RngSample;
let mc: number = 0;
let c: number = 0;
let i: number = 0;
let r: number = 0;
let mr: number = 0;
let ep: number = 0;
let ex: number = 0;
let l: number = 0;

while(pull_id<7){
    index++;
    console.log(test.seePercentage());
    pull = test.rollAndPull(null)!;
    pull_id = pull.event_id;
    switch(pull_id){
        case 7: l++; break;
        case 6: ex++; break;
        case 5: ep++; break;
        case 4: mr++; break;
        case 3: r++; break;
        case 2: i++; break;
        case 1: c++; break;
        case 0: mc++; break;
    }
}


console.log(index);
console.log(pull!.event_name);
console.log(mc, c, i, r, mr, ep, ex, l);


*/
