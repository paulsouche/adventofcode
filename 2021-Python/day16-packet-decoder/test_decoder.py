import unittest

from decoder import parse_hexa_message, part1, part2


class TestDecoder(unittest.TestCase):
    def test_parse_hexa_message_1(self):
        self.assertEqual(parse_hexa_message('D2FE28'), {
            'version': 6,
            'type_ID': 4,
            'value': 2021,
            'length': 21
        })

    def test_parse_hexa_message_2(self):
        self.assertEqual(parse_hexa_message('38006F45291200'), {
            'version': 1,
            'type_ID': 6,
            'length': 49,
            'length_type_ID': 0,
            'value': 1,
            'sub_packets': [
                {
                    'version': 6,
                    'type_ID': 4,
                    'value': 10,
                    'length': 11
                },
                {
                    'version': 2,
                    'type_ID': 4,
                    'value': 20,
                    'length': 16
                }
          ]
        })

    def test_parse_hexa_message_3(self):
        self.assertEqual(parse_hexa_message('EE00D40C823060'), {
            'version': 7,
            'type_ID': 3,
            'length': 51,
            'length_type_ID': 1,
            'value': 3,
            'sub_packets': [
                {
                    'version': 2,
                    'type_ID': 4,
                    'value': 1,
                    'length': 11
                },
                {
                    'version': 4,
                    'type_ID': 4,
                    'value': 2,
                    'length': 11
                },
                {
                    'version': 1,
                    'type_ID': 4,
                    'value': 3,
                    'length': 11
                }
          ]
        })

    def test_parse_hexa_message_4(self):
        self.assertEqual(parse_hexa_message('8A004A801A8002F478'), {
            'version': 4,
            'type_ID': 2,
            'length': 69,
            'length_type_ID': 1,
            'value': 15,
            'sub_packets': [
                {
                    'version': 1,
                    'type_ID': 2,
                    'length': 51,
                    'length_type_ID': 1,
                    'value': 15,
                    'sub_packets': [
                        {
                            'version': 5,
                            'type_ID': 2,
                            'length': 33,
                            'length_type_ID': 0,
                            'value': 15,
                            'sub_packets': [
                                {
                                    'version': 6,
                                    'type_ID': 4,
                                    'value': 15,
                                    'length': 11
                                }
                            ]
                        }
                    ]
                }
            ]
        })

    def test_parse_hexa_message_5(self):
        self.assertEqual(parse_hexa_message('620080001611562C8802118E34'), {
            'version': 3,
            'type_ID': 0,
            'length': 102,
            'length_type_ID': 1,
            'value': 46,
            'sub_packets': [
                {
                    'version': 0,
                    'type_ID': 0,
                    'length': 44,
                    'length_type_ID': 0,
                    'value': 21,
                    'sub_packets': [
                        {
                            'version': 0,
                            'type_ID': 4,
                            'value': 10,
                            'length': 11
                        },
                        {
                            'version': 5,
                            'type_ID': 4,
                            'value': 11,
                            'length': 11
                        }
                    ]
                },
                {
                    'version': 1,
                    'type_ID': 0,
                    'length': 40,
                    'length_type_ID': 1,
                    'value': 25,
                    'sub_packets': [
                        {
                            'version': 0,
                            'type_ID': 4,
                            'value': 12,
                            'length': 11
                        },
                        {
                            'version': 3,
                            'type_ID': 4,
                            'value': 13,
                            'length': 11
                        }
                    ]
                }
            ]
        })

    def test_parse_hexa_message_6(self):
        self.assertEqual(parse_hexa_message('C0015000016115A2E0802F182340'), {
            'version': 6,
            'type_ID': 0,
            'length': 106,
            'length_type_ID': 0,
            'value': 46,
            'sub_packets': [
                {
                    'version': 0,
                    'type_ID': 0,
                    'length': 44,
                    'length_type_ID': 0,
                    'value': 21,
                    'sub_packets': [
                        {
                            'version': 0,
                            'type_ID': 4,
                            'value': 10,
                            'length': 11
                        },
                        {
                            'version': 6,
                            'type_ID': 4,
                            'value': 11,
                            'length': 11
                        }
                    ],
                },
                {
                    'version': 4,
                    'type_ID': 0,
                    'length': 40,
                    'length_type_ID': 1,
                    'value': 25,
                    'sub_packets': [
                        {
                            'version': 7,
                            'type_ID': 4,
                            'value': 12,
                            'length': 11
                        },
                        {
                            'version': 0,
                            'type_ID': 4,
                            'value': 13,
                            'length': 11
                        }
                    ]
                }
            ]
        })

    def test_parse_hexa_message_7(self):
        self.assertEqual(parse_hexa_message('A0016C880162017C3686B18A3D4780'), {
            'version': 5,
            'type_ID': 0,
            'length': 113,
            'length_type_ID': 0,
            'value': 54,
            'sub_packets': [
                {
                    'version': 1,
                    'type_ID': 0,
                    'length': 91,
                    'length_type_ID': 1,
                    'value': 54,
                    'sub_packets': [
                        {
                            'version': 3,
                            'type_ID': 0,
                            'length': 73,
                            'length_type_ID': 1,
                            'value': 54,
                            'sub_packets': [
                                {
                                    'version': 7,
                                    'type_ID': 4,
                                    'value': 6,
                                    'length': 11
                                },
                                {
                                    'version': 6,
                                    'type_ID': 4,
                                    'value': 6,
                                    'length': 11
                                },
                                {
                                    'version': 5,
                                    'type_ID': 4,
                                    'value': 12,
                                    'length': 11
                                },
                                {
                                    'version': 2,
                                    'type_ID': 4,
                                    'value': 15,
                                    'length': 11
                                },
                                {
                                    'version': 2,
                                    'type_ID': 4,
                                    'value': 15,
                                    'length': 11
                                }
                            ]
                        }
                    ]
                }
            ]
        })

    def test_part1_1(self):
        self.assertEqual(part1('8A004A801A8002F478'), 16)

    def test_part1_2(self):
        self.assertEqual(part1('620080001611562C8802118E34'), 12)

    def test_part1_3(self):
        self.assertEqual(part1('C0015000016115A2E0802F182340'), 23)

    def test_part1_4(self):
        self.assertEqual(part1('A0016C880162017C3686B18A3D4780'), 31)

    def test_part2_1(self):
        self.assertEqual(part2('C200B40A82'), 3)

    def test_part2_2(self):
        self.assertEqual(part2('04005AC33890'), 54)

    def test_part2_3(self):
        self.assertEqual(part2('880086C3E88112'), 7)

    def test_part2_4(self):
        self.assertEqual(part2('CE00C43D881120'), 9)

    def test_part2_5(self):
        self.assertEqual(part2('D8005AC2A8F0'), 1)

    def test_part2_6(self):
        self.assertEqual(part2('F600BC2D8F'), 0)

    def test_part2_7(self):
        self.assertEqual(part2('9C005AC2F8F0'), 0)

    def test_part2_8(self):
        self.assertEqual(part2('9C0141080250320F1802104A08'), 1)
