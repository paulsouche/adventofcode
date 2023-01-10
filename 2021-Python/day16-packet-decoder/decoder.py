import operator
from functools import reduce

HEXA_TO_BINARY_MAP = {
    '0': '0000',
    '1': '0001',
    '2': '0010',
    '3': '0011',
    '4': '0100',
    '5': '0101',
    '6': '0110',
    '7': '0111',
    '8': '1000',
    '9': '1001',
    'A': '1010',
    'B': '1011',
    'C': '1100',
    'D': '1101',
    'E': '1110',
    'F': '1111',
}

def parse_binary_packet(binary_packet):
    packet = {}
    packet['version'] = int('0' + binary_packet[0:3], 2)
    packet['type_ID'] = int('0' + binary_packet[3:6], 2)
    # Literal value
    if packet['type_ID'] == 4:
        ind = 6
        value = ''
        while True:
            value += binary_packet[ind + 1:ind + 5]
            if binary_packet[ind] == '0':
                break
            ind += 5
        packet['value'] = int(value, 2)
        packet['length'] = ind + 5
    #Operator
    else:
        packet['sub_packets'] = []
        packet['length_type_ID'] = int(binary_packet[6])
        if packet['length_type_ID'] == 0:
            length = int(binary_packet[7:22], 2)
            packet['length'] = 22 + length
            read = 0
            while read < length:
                sub_packet = parse_binary_packet(binary_packet[22 + read:22 + read + length])
                packet['sub_packets'].append(sub_packet)
                read += sub_packet['length']
        else:
            number_of_sub_packets = int(binary_packet[7:18], 2)
            packet['length'] = 18
            while number_of_sub_packets > 0:
                sub_packet = parse_binary_packet(binary_packet[packet['length']:len(binary_packet)])
                packet['sub_packets'].append(sub_packet)
                packet['length'] += sub_packet['length']
                number_of_sub_packets -= 1

        if packet['type_ID'] == 0:
            packet['value'] = sum(list(map(lambda p: p['value'], packet['sub_packets'])))
        elif packet['type_ID'] == 1:
            packet['value'] = reduce(operator.mul, list(map(lambda p: p['value'], packet['sub_packets'])), 1)
        elif packet['type_ID'] == 2:
            packet['value'] = min(list(map(lambda p: p['value'], packet['sub_packets'])))
        elif packet['type_ID'] == 3:
            packet['value'] = max(list(map(lambda p: p['value'], packet['sub_packets'])))
        elif packet['type_ID'] == 5:
            packet['value'] = 1 if packet['sub_packets'][0]['value'] > packet['sub_packets'][1]['value'] else 0
        elif packet['type_ID'] == 6:
            packet['value'] = 1 if packet['sub_packets'][0]['value'] < packet['sub_packets'][1]['value'] else 0
        elif packet['type_ID'] == 7:
            packet['value'] = 1 if packet['sub_packets'][0]['value'] == packet['sub_packets'][1]['value'] else 0

    return packet

def parse_hexa_message(message):
    return parse_binary_packet(''.join(list(map(lambda char: HEXA_TO_BINARY_MAP[char], message))))

def flat_map(packet, packets):
    packets.append(packet)
    if not 'sub_packets' in packet:
        return packets
    for sub_packet in packet['sub_packets']:
        flat_map(sub_packet, packets)
    return packets

def part1(message):
    root_packet = parse_hexa_message(message)
    # Does not work with a default array in flat_map in f****** python !!
    return sum(list(map(lambda p: p['version'], flat_map(root_packet, []))))

def part2(message):
    root_packet = parse_hexa_message(message)
    return root_packet['value']
