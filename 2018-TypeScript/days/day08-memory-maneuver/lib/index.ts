export default (input: string) => {
  const tree = input.split(' ').map((c) => +c);

  let sum = 0;

  const treatNode = (ind: number) => {
    const childrenNum = tree[ind];
    const metadatasNum = tree[++ind];
    const chidrenValues: number[] = [];
    let nodeValue: number;

    let child = 0;
    while (child < childrenNum) {
      ({ ind, nodeValue } = treatNode(++ind));
      chidrenValues.push(nodeValue);
      child++;
    }

    let nodeMetadataSum = 0;
    let metaData = 0;
    nodeValue = 0;
    while (metaData < metadatasNum) {
      const meta = tree[++ind];
      nodeMetadataSum += meta;
      nodeValue += (chidrenValues[meta - 1] || 0);
      metaData++;
    }

    if (childrenNum === 0) {
      nodeValue = nodeMetadataSum;
    }

    sum += nodeMetadataSum;

    return {
      ind,
      nodeValue,
    };
  };

  const { nodeValue: value } = treatNode(0);

  return {
    sum,
    value,
  };
};
